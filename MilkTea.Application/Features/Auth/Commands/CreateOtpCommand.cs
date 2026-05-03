using FluentValidation;
using MilkTea.Application.Features.Auth.Models.Results;
using MilkTea.Application.Features.Configuration.Abstractions.Services;
using MilkTea.Application.Features.User.Abstractions.Services;
using MilkTea.Application.Ports.Notification;
using MilkTea.Domain.Auth.Exceptions;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Domain.Auth.ValueObjects;
using MilkTea.Domain.Common.Constants;
using MilkTea.Shared.Templates;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Auth.Commands
{
    public class CreateOtpCommand : ICommand<CreateOtpResult>
    {
        public string Email { get; set; } = string.Empty;
        public string Function { get; set; } = string.Empty;
    }

    public sealed class CreateOtpCommandValidator : AbstractValidator<CreateOtpCommand>
    {
        public CreateOtpCommandValidator() {
            RuleFor(x => x.Function)
                .Must(SessionFunction.IsValid)
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(CreateOtpCommand.Function));
        }
    }

    public class CreateOtpCommandHandler(IAuthUnitOfWork authUnitOfWork,
                                            IConfigurationService configurationService,
                                            IUserService userService,
                                            INotificationSender notificationSender) : ICommandHandler<CreateOtpCommand, CreateOtpResult>
    {
        private readonly IAuthUnitOfWork _vAuthUnitOfWork = authUnitOfWork;
        private readonly IConfigurationService _vConfigurationService = configurationService;
        private readonly IUserService _vUserService = userService;
        private readonly INotificationSender _vNotificationSender = notificationSender;

        public async Task<CreateOtpResult> Handle(CreateOtpCommand command, CancellationToken cancellationToken)
        {
            var result = new CreateOtpResult();

            // 1. Validate email exists
            var userId = await _vUserService.GetUserIdByEmailAsync(command.Email, cancellationToken);
            if (userId is null)
                return SendError(result, ErrorCode.E0001, nameof(command.Email));

            // 2. Get Session expiration time from ConfigurationService
            var expirationSesionMinutes = await _vConfigurationService.GetSessionExpirationMinutesAsync(cancellationToken);

            // 3. Get OTP expiration time from ConfigurationService
            var expirationOtpMinutes = await _vConfigurationService.GetOtpExpirationMinutesAsync(cancellationToken);

            // 4. Start transaction
            await _vAuthUnitOfWork.BeginTransactionAsync(cancellationToken);
            Domain.Auth.Entities.SessionEntity session;
            Domain.Auth.Entities.OtpEntity otp;
            try
            {
                // 4. Create new Session entity
                var expiresSessionDate = DateTime.Now.AddMinutes(expirationSesionMinutes);
                session = Domain.Auth.Entities.SessionEntity.Create(
                    email: command.Email,
                    channel: Channel.Email,
                    function: SessionFunction.Create(command.Function),
                    expiresDate: expiresSessionDate);

                await _vAuthUnitOfWork.Sessions.AddAsync(session, cancellationToken);
                await _vAuthUnitOfWork.SaveChangesAsync(cancellationToken);


                // 5. Create new OTP entity linked to session
                var expiresOtpDate = DateTime.Now.AddMinutes(expirationOtpMinutes);
                otp = Domain.Auth.Entities.OtpEntity.Create(
                    sessionId: session.Id,
                    channel: Channel.Email,
                    expiredDate: expiresOtpDate);
                await _vAuthUnitOfWork.Otps.AddAsync(otp, cancellationToken);
                await _vAuthUnitOfWork.SaveChangesAsync(cancellationToken);

                // 6. Send email with OTP
                await _vNotificationSender.SendAsync(
                    new NotificationRequest(
                        NotificationChannel.Email,
                        command.Email,
                        $"Mã xác thực của bạn: {otp.OtpCode}",
                        OtpEmail.BuildOtpTemplate(otp.OtpCode, expirationOtpMinutes, "Mã OTP để xác thực", "MilkTea Shop")),
                    cancellationToken);

                await _vAuthUnitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch (InvalidFunctionSessionException)
            {
                await _vAuthUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0036, nameof(command.Function));
            }
            catch (InvalidChannelException)
            {
                await _vAuthUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0036, "Channel");
            }
            catch
            {
                await _vAuthUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E9999, "SendOTP");
            }

            result.SessionId = session.Id;
            result.ExpiresAt = otp.ExpiredDate;
            return result;
        }

        private static CreateOtpResult SendError(CreateOtpResult result, string errorCode, params string[] values)
        {
            if (values is { Length: > 0 })
                result.ResultData.Add(errorCode, values.ToList());
            return result;
        }
    }
}
