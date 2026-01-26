using MediatR;
using MilkTea.Application.Features.Users.Commands;
using MilkTea.Application.Ports.Users;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Domain.SharedKernel.Repositories;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.Features.Users.Commands;

public sealed class LogoutCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
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

        // Get user
        var user = await userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            return result;
        }

        await unitOfWork.BeginTransactionAsync();
        try
        {
            // Revoke refresh token using domain method
            user.RevokeRefreshToken(command.RefreshToken, userId);
            await userRepository.UpdateAsync(user);

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
