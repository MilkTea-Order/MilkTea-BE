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

    public class CreateOtpCommandHandler(
        IAuthUnitOfWork authUnitOfWork,
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
            // 1. Get OTP expiration time from ConfigurationService
            var expirationMinutes = await _vConfigurationService.GetSessionExpirationMinutesAsync(cancellationToken);

            // 2. Start transaction
            await _vAuthUnitOfWork.BeginTransactionAsync(cancellationToken);
            Domain.Auth.Entities.SessionEntity session;
            Domain.Auth.Entities.OtpEntity otp;
            try
            {
                // 3. Create new Session entity
                var expiresDate = DateTime.Now.AddMinutes(expirationMinutes);
                session = Domain.Auth.Entities.SessionEntity.Create(
                    email: command.Email,
                    channel: Channel.Email,
                    function: SessionFunction.Create(command.Function),
                    expiresDate: expiresDate);

                await _vAuthUnitOfWork.Sessions.AddAsync(session, cancellationToken);
                await _vAuthUnitOfWork.SaveChangesAsync(cancellationToken);

                // 4. Create new OTP entity linked to session
                otp = Domain.Auth.Entities.OtpEntity.Create(
                    sessionId: session.Id,
                    channel: Channel.Email,
                    expiredDate: expiresDate);
                await _vAuthUnitOfWork.Otps.AddAsync(otp, cancellationToken);
                await _vAuthUnitOfWork.SaveChangesAsync(cancellationToken);

                // 5. Send email with OTP
                await _vNotificationSender.SendAsync(
                    new NotificationRequest(
                        NotificationChannel.Email,
                        command.Email,
                        "Mã OTP của bạn",
                        OtpEmail.BuildOtpTemplate(otp.OtpCode, expirationMinutes, "Mã OTP để xác thực", "MilkTea Shop")),
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
