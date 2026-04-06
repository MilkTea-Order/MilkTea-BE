using MilkTea.Application.Features.Auth.Models.Results;
using MilkTea.Application.Features.Configuration.Abstractions.Services;
using MilkTea.Application.Features.User.Abstractions.Services;
using MilkTea.Application.Ports.Notification;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Domain.Common.Constants;
using MilkTea.Shared.Domain.Constants;
using MilkTea.Shared.Templates;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Auth.Commands
{
    public class ForgotPasswordCommand : ICommand<ForgetPasswordResult>
    {
        public string Email { get; set; } = string.Empty;
    }

    public class ForgotPasswordCommandHandler(
        IAuthUnitOfWork authUnitOfWork,
        IConfigurationService configurationService,
        IUserService userService,
        INotificationSender notificationSender) : ICommandHandler<ForgotPasswordCommand, ForgetPasswordResult>
    {
        private readonly IAuthUnitOfWork _vAuthUnitOfWork = authUnitOfWork;
        private readonly IConfigurationService _vConfigurationService = configurationService;
        private readonly IUserService _vUserService = userService;
        private readonly INotificationSender _vNotificationSender = notificationSender;

        public async Task<ForgetPasswordResult> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            var result = new ForgetPasswordResult();

            // 1. Get user ID by email
            var userId = await _vUserService.GetUserIdByEmailAsync(command.Email, cancellationToken);

            if (userId == null)
            {

                return SendError(result, ErrorCode.E0001, nameof(command.Email));
            }

            // 2. Get OTP expiration time from ConfigurationService
            var expirationMinutes = await _vConfigurationService.GetOtpExpirationMinutesAsync(cancellationToken);

            // 3. Start transaction
            await _vAuthUnitOfWork.BeginTransactionAsync(cancellationToken);
            Domain.Auth.Entities.OtpEntity otp;
            try
            {
                // 4. Create new OTP entity
                otp = Domain.Auth.Entities.OtpEntity.Create(
                   email: command.Email,
                   otpType: Denifinitions.OTP_TYPE_FORGOT_PASSWORD,
                   createdBy: userId.Value);

                await _vAuthUnitOfWork.Otps.AddAsync(otp, cancellationToken);
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
            catch
            {
                await _vAuthUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E9999, "SendOTP");
            }
            result.ExpiresAt = otp.OTPDate.AddMinutes(expirationMinutes);
            return result;
        }
        private static ForgetPasswordResult SendError(ForgetPasswordResult result, string errorCode, params string[] values)
        {
            if (values is { Length: > 0 })
                result.ResultData.Add(errorCode, values.ToList());
            return result;
        }
    }
}
