using FluentValidation;
using MilkTea.Application.Features.Auth.Models.Results;
using MilkTea.Application.Ports.Hash.Password;
using MilkTea.Domain.Auth.Exceptions;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Domain.Common.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Auth.Commands;

public class ResetPasswordCommand : ICommand<ResetPasswordResult>
{
    public string ResetPasswordToken { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.ResetPasswordToken)
            .NotEmpty()
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName(nameof(ResetPasswordCommand.ResetPasswordToken));

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithErrorCode(ErrorCode.E0036)
            .MinimumLength(5)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName(nameof(ResetPasswordCommand.NewPassword));

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.NewPassword)
            .WithErrorCode(ErrorCode.E0002)
            .OverridePropertyName(nameof(ResetPasswordCommand.ConfirmPassword));
    }
}


public sealed class ResetPasswordCommandHandler(IAuthUnitOfWork authUnitOfWork,
                                                 IPasswordHasher passwordHasher
) : ICommandHandler<ResetPasswordCommand, ResetPasswordResult>
{
    private readonly IAuthUnitOfWork _vAuthUnitOfWork = authUnitOfWork;
    private readonly IPasswordHasher _vPasswordHasher = passwordHasher;

    public async Task<ResetPasswordResult> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var result = new ResetPasswordResult();

        // 1. Find valid reset password token
        var resetToken = await _vAuthUnitOfWork.ResetPasswordTokens
            .GetValidTokenForUpdateAsync(command.ResetPasswordToken, cancellationToken);

        if (resetToken is null)
        {
            return SendError(result, ErrorCode.E0001, nameof(command.ResetPasswordToken));
        }

        // 2. Check if token is expired
        if (resetToken.IsExpired)
        {
            return SendError(result, ErrorCode.E0043, nameof(command.ResetPasswordToken));
        }

        // 3. Check if token has been used
        if (resetToken.IsUsed || resetToken.IsRevoked)
        {
            return SendError(result, ErrorCode.E0001, nameof(command.ResetPasswordToken));
        }

        // 4. Get user by ID from token
        var user = await _vAuthUnitOfWork.Users
            .GetByIdForUpdateAsync(resetToken.UserId, cancellationToken);

        if (user is null)
        {
            return SendError(result, ErrorCode.E0001, "user");
        }

        // 5. Hash new password
        var newPasswordHash = _vPasswordHasher.HashPassword(command.NewPassword);

        // 6. Begin transaction
        await _vAuthUnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // Update password
            user.UpdatePassword(newPasswordHash, resetToken.UserId);

            // Mark token as used
            resetToken.MarkAsUsed();

            // Commit transaction
            await _vAuthUnitOfWork.CommitTransactionAsync(cancellationToken);
            return result;
        }
        catch (PasswordUsedException)
        {
            await _vAuthUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E0002, nameof(command.NewPassword));
        }
        catch (Exception)
        {
            await _vAuthUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E9999, "ResetPassword");
        }
    }

    private static ResetPasswordResult SendError(ResetPasswordResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
