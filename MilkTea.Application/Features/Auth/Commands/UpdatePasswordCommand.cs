using FluentValidation;
using MilkTea.Application.Features.Auth.Models.Results;
using MilkTea.Application.Ports.Hash.Password;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Domain.Common.Constants;
using MilkTea.Shared.Domain.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Auth.Commands;

public class UpdatePasswordCommand : ICommand<UpdatePasswordResult>
{
    public string Password { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;
}

public sealed class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.Password)
                  .NotEmpty().WithErrorCode(ErrorCode.E0001)
                  .MinimumLength(5).WithErrorCode(ErrorCode.E0001)
                  .OverridePropertyName(nameof(UpdatePasswordCommand.Password));
        RuleFor(x => x.NewPassword)
                  .NotEmpty().WithErrorCode(ErrorCode.E0001)
                  .MinimumLength(5).WithErrorCode(ErrorCode.E0001)
                  .NotEqual(x => x.Password).WithErrorCode(ErrorCode.E0002)
                  .OverridePropertyName(nameof(UpdatePasswordCommand.NewPassword));
        RuleFor(x => x.ConfirmPassword)
                    .Equal(x => x.NewPassword).WithErrorCode(ErrorCode.E0002)
                    .OverridePropertyName(nameof(UpdatePasswordCommand.ConfirmPassword));
    }
}
public sealed class UpdatePasswordCommandHandler(IAuthUnitOfWork authUnitOfWork,
                                                    IIdentifyServicePorts currentUser,
                                                    IPasswordHasher passwordHasher
                                                    ) : ICommandHandler<UpdatePasswordCommand, UpdatePasswordResult>

{
    private readonly IAuthUnitOfWork _vAuthUnitOfWork = authUnitOfWork;
    private readonly IIdentifyServicePorts _vCurrentUser = currentUser;
    private readonly IPasswordHasher _vPasswordHasher = passwordHasher;
    public async Task<UpdatePasswordResult> Handle(UpdatePasswordCommand command, CancellationToken cancellationToken)
    {
        var result = new UpdatePasswordResult();
        var userId = _vCurrentUser.UserId;

        var user = await _vAuthUnitOfWork.Users.GetByIdForUpdateAsync(userId, cancellationToken);
        // Not Authorized to update password of other users => logout
        if (user is null)
        {
            result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            return result;
        }

        if (!_vPasswordHasher.VerifyHashedPassword(user.Password.value, command.Password))
        {
            return SendError(result, ErrorCode.E0001, "password");
        }

        // Encrypt new password
        var newPasswordHash = _vPasswordHasher.HashPassword(command.NewPassword);

        await _vAuthUnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // Update password using domain method
            user.UpdatePassword(newPasswordHash, userId);

            // Commit transaction 
            await _vAuthUnitOfWork.CommitTransactionAsync(cancellationToken);
            return result;
        }
        catch (Exception)
        {
            await _vAuthUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E9999, "UpdatePassword");
        }
    }

    private static UpdatePasswordResult SendError(UpdatePasswordResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}

