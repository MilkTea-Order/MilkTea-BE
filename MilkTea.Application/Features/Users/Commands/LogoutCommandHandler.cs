using MediatR;
using MilkTea.Application.Features.Users.Commands;
using MilkTea.Application.Ports.Users;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.SharedKernel.Repositories;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.Features.Users.Commands;

public sealed class LogoutCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser) : IRequestHandler<LogoutCommand, LogoutResult>
{
    public async Task<LogoutResult> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        var result = new LogoutResult();
        var userId = currentUser.UserId;

        if (string.IsNullOrWhiteSpace(command.RefreshToken))
        {
            result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            return result;
        }

        await unitOfWork.BeginTransactionAsync();
        try
        {
            // Load user with tracking for update
            var user = await unitOfWork.Users.GetByIdForUpdateAsync(userId);
            if (user is null)
            {
                await unitOfWork.RollbackTransactionAsync();
                result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
                return result;
            }

            // Revoke refresh token using domain method (managed through User aggregate)
            user.RevokeRefreshToken(command.RefreshToken, userId);

            // Commit transaction (SaveChangesAsync is called inside CommitTransactionAsync)
            await unitOfWork.CommitTransactionAsync();
            return result;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync();
            result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            return result;
        }
    }
}
