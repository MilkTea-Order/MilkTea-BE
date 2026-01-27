using MediatR;
using MilkTea.Application.Features.Users.Commands;
using MilkTea.Application.Models.Errors;
using MilkTea.Application.Ports.Users;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.SharedKernel.Repositories;
using MilkTea.Shared.Utils;

namespace MilkTea.Application.Features.Users.Commands;

public sealed class UpdatePasswordCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser) : IRequestHandler<UpdatePasswordCommand, UpdatePasswordResult>
{
    public async Task<UpdatePasswordResult> Handle(UpdatePasswordCommand command, CancellationToken cancellationToken)
    {
        var result = new UpdatePasswordResult();
        var userId = currentUser.UserId;

        // Load user (read-only for validation)
        var user = await unitOfWork.Users.GetByIdAsync(userId);
        if (user is null)
            return SendError(result, ErrorCode.E0001, nameof(command));

        // Verify current password
        if (!RC2Helper.VerifyPasswordRC2(user.Password.value, command.Password))
            return SendError(result, ErrorCode.E0001, "password");

        // Validate new password is different
        if (RC2Helper.VerifyPasswordRC2(user.Password.value, command.NewPassword))
            return SendError(result, ErrorCode.E0012, "newPassword");

        await unitOfWork.BeginTransactionAsync();
        try
        {
            // Load user with tracking for update
            var userForUpdate = await unitOfWork.Users.GetByIdForUpdateAsync(userId);
            if (userForUpdate is null)
            {
                await unitOfWork.RollbackTransactionAsync();
                return SendError(result, ErrorCode.E0001, nameof(command));
            }

            // Encrypt new password
            var newPasswordHash = RC2Helper.EncryptByRC2(command.NewPassword);
            
            // Update password using domain method
            userForUpdate.UpdatePassword(newPasswordHash, userId);

            // Commit transaction (SaveChangesAsync is called inside CommitTransactionAsync)
            await unitOfWork.CommitTransactionAsync();

            return result;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync();
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
