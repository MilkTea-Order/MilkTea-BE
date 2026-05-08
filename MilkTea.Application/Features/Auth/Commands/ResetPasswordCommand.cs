using FluentValidation;
using MilkTea.Application.Features.Auth.Abstractions.Queries;
using MilkTea.Application.Features.Auth.Models.Results;
using MilkTea.Application.Features.User.Abstractions.Queries;
using MilkTea.Application.Ports.Hash.Password;
using MilkTea.Domain.Auth.Exceptions;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Domain.Common.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Auth.Commands;

public class ResetPasswordCommand : ICommand<ResetPasswordResult>
{
    public string Email { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName(nameof(ResetPasswordCommand.Email));

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
                                                 IPasswordHasher passwordHasher,
                                                 IUserQuery userQuery,
                                                 IAuthQuery authQuery) : ICommandHandler<ResetPasswordCommand, ResetPasswordResult>
{
    private readonly IAuthUnitOfWork _vAuthUnitOfWork = authUnitOfWork;
    private readonly IPasswordHasher _vPasswordHasher = passwordHasher;
    private readonly IUserQuery _vUserQuery = userQuery;
    private readonly IAuthQuery _vAuthQuery = authQuery;

    public async Task<ResetPasswordResult> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var result = new ResetPasswordResult();

        // 1. Find employee by email
        var employeeId = await _vUserQuery.GetEmployeeIdByEmailAsync(command.Email, cancellationToken);
        if (employeeId is null)
        {
            return SendError(result, ErrorCode.E0001, nameof(command.Email));
        }

        // 2. Get user ID by employee ID
        var userId = await _vAuthQuery.GetUserIdByEmployeeIdAsync(employeeId.Value, cancellationToken);
        if (userId is null)
        {
            return SendError(result, ErrorCode.E0001, nameof(command.Email));
        }

        // 3. Get user by ID for update
        var user = await _vAuthUnitOfWork.Users.GetByIdForUpdateAsync(userId.Value, cancellationToken);

        if (user is null)
        {
            return SendError(result, ErrorCode.E0001, nameof(command.Email));
        }

        // 4. Hash new password
        var newPasswordHash = _vPasswordHasher.HashPassword(command.NewPassword);

        // 5. Begin transaction
        await _vAuthUnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // Update password
            user.UpdatePassword(newPasswordHash, userId.Value);

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
