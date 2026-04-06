using FluentValidation;
using MilkTea.Application.Features.Auth.Abstractions.Queries;
using MilkTea.Application.Features.Auth.Models.Results;
using MilkTea.Application.Features.Configuration.Abstractions.Services;
using MilkTea.Application.Features.User.Abstractions.Services;
using MilkTea.Application.Ports.Authentication.JWTPorts;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Domain.Common.Constants;
using MilkTea.Shared.Domain.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Auth.Commands
{
    public class VerifyOtpCommand : ICommand<VerifyOtpResult>
    {
        public string Email { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }

    public sealed class VerifyOtpCommandValidator : AbstractValidator<VerifyOtpCommand>
    {
        public VerifyOtpCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName(nameof(VerifyOtpCommand.Email));

            RuleFor(x => x.Otp)
                .NotEmpty()
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName(nameof(VerifyOtpCommand.Otp));
        }
    }

    public class VerifyOtpCommandHandler(IAuthUnitOfWork authUnitOfWork,
                                            IConfigurationService configurationService,
                                            IUserService userService,
                                            IAuthQuery authQuery,
                                            IJWTServicePort jwtService) : ICommandHandler<VerifyOtpCommand, VerifyOtpResult>
    {
        private readonly IAuthUnitOfWork _vAuthUnitOfWork = authUnitOfWork;
        private readonly IConfigurationService _vConfigurationService = configurationService;
        private readonly IUserService _vUserService = userService;
        private readonly IAuthQuery _vAuthQuery = authQuery;
        private readonly IJWTServicePort _vJwtService = jwtService;

        public async Task<VerifyOtpResult> Handle(VerifyOtpCommand command, CancellationToken cancellationToken)
        {
            var result = new VerifyOtpResult();

            // 1. Get employee ID by email
            var employeeId = await _vUserService.GetUserIdByEmailAsync(command.Email, cancellationToken);

            if (employeeId == null)
            {
                return SendError(result, ErrorCode.E0001, nameof(command.Email));
            }

            // 2. Check if user exists with this employee ID
            var user = await _vAuthUnitOfWork.Users.GetByIdAsync(employeeId.Value, cancellationToken);

            if (user == null)
            {
                return SendError(result, ErrorCode.E0001, nameof(command.Email));
            }

            // 3. Get the latest OTP for this email and type via AuthQuery
            var otp = await _vAuthQuery.GetLatestOtpByEmailAndTypeAsync(
                command.Email,
                Denifinitions.OTP_TYPE_FORGOT_PASSWORD,
                cancellationToken);

            if (otp == null)
            {
                return SendError(result, ErrorCode.E0001, nameof(command.Otp));
            }

            // 4. Get OTP configuration
            var expirationMinutes = await _vConfigurationService.GetOtpExpirationMinutesAsync(cancellationToken);
            var maxAttempts = await _vConfigurationService.GetOtpMaxAttemptsAsync(cancellationToken);

            // 5. Check if OTP is expired
            if (otp.IsExpired(expirationMinutes))
            {
                return SendError(result, ErrorCode.E0043, nameof(command.Otp));
            }

            // 6. Check if OTP has exceeded max attempts
            if (otp.IsMaxAttemptsReached(maxAttempts + 1))
            {
                return SendError(result, ErrorCode.E0044, nameof(command.Otp));
            }

            // 7. Check if OTP code matches
            if (otp.OtpCode != command.Otp)
            {
                return SendError(result, ErrorCode.E0001, nameof(command.Otp));
            }

            // 8. Get reset password token expiration minutes
            var resetTokenExpirationMinutes = await _vConfigurationService.GetResetPasswordTokenExpirationMinutesAsync(cancellationToken);

            // 9. Generate reset password token via JWT service
            var (resetToken, expiresAt) = _vJwtService.CreateResetPasswordToken(resetTokenExpirationMinutes);

            // 10. Create and save ResetPasswordToken entity
            await _vAuthUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var resetPasswordToken = Domain.Auth.Entities.ResetPasswordTokenEntity.Create(
                    userId: user.Id,
                    token: resetToken,
                    expiresAt: expiresAt);

                result.Token = resetToken;
                result.Expiration = expiresAt;

                await _vAuthUnitOfWork.ResetPasswordTokens.AddAsync(resetPasswordToken, cancellationToken);
                await _vAuthUnitOfWork.CommitTransactionAsync(cancellationToken);

            }
            catch
            {
                await _vAuthUnitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            return result;
        }

        private static VerifyOtpResult SendError(VerifyOtpResult result, string errorCode, params string[] values)
        {
            if (values is { Length: > 0 })
                result.ResultData.Add(errorCode, values.ToList());
            return result;
        }
    }
}
