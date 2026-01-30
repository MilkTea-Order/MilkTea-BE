using MilkTea.Application.Features.Users.Commands;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Application.Ports.Hash.Password;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.SharedKernel.Repositories;
using MilkTea.Shared.Domain.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Users.Handles;

public sealed class UpdatePasswordCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IPasswordHasher passwordHasher
    ) : ICommandHandler<UpdatePasswordCommand, UpdatePasswordResult>

{
    public async Task<UpdatePasswordResult> Handle(UpdatePasswordCommand command, CancellationToken cancellationToken)
    {
        var result = new UpdatePasswordResult();
        var userId = currentUser.UserId;

        var user = await unitOfWork.Users.GetByIdForUpdateAsync(userId, cancellationToken);
        // Not Authorized to update password of other users => logout
        if (user is null)
        {
            result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            return result;
        }

        if (!passwordHasher.VerifyHashedPassword(user.Password.value, command.Password))
        {
            return SendError(result, ErrorCode.E0001, "password");
        }

        // Encrypt new password
        var newPasswordHash = passwordHasher.HashPassword(command.NewPassword);

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // Update password using domain method
            user.UpdatePassword(newPasswordHash, userId);

            // Commit transaction 
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return result;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
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
