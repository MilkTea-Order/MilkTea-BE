using FluentValidation;
using MilkTea.Application.Features.Auth.Abstractions.Queries;
using MilkTea.Application.Features.Auth.Models.Results;
using MilkTea.Application.Features.User.Abstractions.Queries;
using MilkTea.Application.Ports.Authentication.JWTPorts;
using MilkTea.Domain.Auth.Entities;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Domain.Common.Constants;
using MilkTea.Shared.Domain.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Auth.Commands
{
    public class VerifyOtpCommand : ICommand<VerifyOtpResult>
    {
        public int SessionId { get; set; }
        public string OtpCode { get; set; } = string.Empty;
    }

    public sealed class VerifyOtpCommandValidator : AbstractValidator<VerifyOtpCommand>
    {
        public VerifyOtpCommandValidator()
        {
            RuleFor(x => x.SessionId)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName(nameof(VerifyOtpCommand.SessionId));

            RuleFor(x => x.OtpCode)
                .NotEmpty()
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName(nameof(VerifyOtpCommand.OtpCode));
        }
    }

    public class VerifyOtpCommandHandler(
        IAuthUnitOfWork authUnitOfWork,
        IOtpQuery otpQuery,
        IJWTServicePort jwtServicePort,
        IUserQuery userQuery,
        IAuthQuery authQuery) : ICommandHandler<VerifyOtpCommand, VerifyOtpResult>
    {
        private readonly IAuthUnitOfWork _vAuthUnitOfWork = authUnitOfWork;
        private readonly IOtpQuery _vOtpQuery = otpQuery;
        private readonly IJWTServicePort _vJwtServicePort = jwtServicePort;
        private readonly IUserQuery _vUserQuery = userQuery;
        private readonly IAuthQuery _vAuthQuery = authQuery;

        public async Task<VerifyOtpResult> Handle(VerifyOtpCommand command, CancellationToken cancellationToken)
        {
            var result = new VerifyOtpResult();

            // 1. Get session by ID (without tracking)
            var session = await _vAuthUnitOfWork.Sessions.GetByIdAsync(command.SessionId, cancellationToken);
            if (session is null)
            {
                return SendError(result, ErrorCode.E0001, nameof(command.SessionId));
            }

            // 2. Check if session is expired
            if (session.IsExpired)
            {
                return SendError(result, ErrorCode.E0043, nameof(command.SessionId));
            }

            // 3. Check if session is already verified
            if (session.IsVerified)
            {
                return SendError(result, ErrorCode.E0042, nameof(command.SessionId));
            }

            // 4. Get the latest valid OTP for this session (using Query for read operation)
            var otp = await _vOtpQuery.GetLatestValidOtpBySessionIdAsync(command.SessionId, cancellationToken);
            if (otp is null)
            {
                return SendError(result, ErrorCode.E0001, nameof(command.OtpCode));
            }

            // 5. Check if OTP code matches
            if (otp.OtpCode != command.OtpCode)
            {
                return SendError(result, ErrorCode.E0004, nameof(command.OtpCode));
            }

            // 6. Get employee ID by email
            var employeeId = await _vUserQuery.GetEmployeeIdByEmailAsync(session.Email, cancellationToken);
            if (employeeId is null)
            {
                return SendError(result, ErrorCode.E0001, "User");
            }

            // 7. Get user ID by employee ID
            var userId = await _vAuthQuery.GetUserIdByEmployeeIdAsync(employeeId.Value, cancellationToken);
            if (userId is null)
            {
                return SendError(result, ErrorCode.E0001, "User");
            }

            // 8. Get user for update
            var user = await _vAuthUnitOfWork.Users.GetByIdForUpdateAsync(userId.Value, cancellationToken);
            if (user is null)
            {
                return SendError(result, ErrorCode.E9999, "User");
            }

            // 9. Get session with tracking to update
            var sessionForUpdate = await _vAuthUnitOfWork.Sessions.GetByIdForUpdateAsync(command.SessionId, cancellationToken);
            if (sessionForUpdate is null)
            {
                return SendError(result, ErrorCode.E9999, nameof(command.SessionId));
            }

            // 10. Mark session as verified and create reset password token
            await _vAuthUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                sessionForUpdate.MarkAsVerified();

                // 11. Generate reset password token
                var (resetToken, expiresAt) = _vJwtServicePort.CreateResetPasswordToken(minutes: 15);

                // 12. Save reset password token to database
                var resetPasswordToken = ResetPasswordTokenEntity.Create(
                    userId: user.Id,
                    token: resetToken,
                    expiresAt: expiresAt);
                await _vAuthUnitOfWork.ResetPasswordTokens.AddAsync(resetPasswordToken, cancellationToken);

                await _vAuthUnitOfWork.SaveChangesAsync(cancellationToken);
                await _vAuthUnitOfWork.CommitTransactionAsync(cancellationToken);

                result.IsVerified = true;
                result.ResetToken = resetToken;
                result.ResetTokenExpiresAt = expiresAt;
                return result;
            }
            catch
            {
                await _vAuthUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E9999, "VerifyOtp");
            }
        }

        private static VerifyOtpResult SendError(VerifyOtpResult result, string errorCode, params string[] values)
        {
            if (values is { Length: > 0 })
                result.ResultData.Add(errorCode, values.ToList());
            return result;
        }
    }
}
