using MediatR;
using MilkTea.Application.Features.Users.Commands;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.SharedKernel.Repositories;
using MilkTea.Shared.Domain.Constants;
using MilkTea.Shared.Extensions;

namespace MilkTea.Application.Features.Users.Handles;

public sealed class LogoutCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser) : IRequestHandler<LogoutCommand, LogoutResult>
{
    public async Task<LogoutResult> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        var result = new LogoutResult();
        var userId = currentUser.UserId;

        if (command.RefreshToken.IsNullOrWhiteSpace())
        {
            result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            return result;
        }

        // Load user with tracking for update
        var user = await unitOfWork.Users.GetByIdForUpdateAsync(userId, cancellationToken);
        if (user is null)
        {
            result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            return result;
        }

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // Revoke refresh token
            user.RevokeRefreshToken(command.RefreshToken, userId);

            // Commit transaction 
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return result;
        }
        catch (Exception)
        {
            // Rollback transaction on error and logout
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            return result;
        }
    }
}
