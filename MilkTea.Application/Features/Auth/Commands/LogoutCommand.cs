using FluentValidation;
using MediatR;
using MilkTea.Application.Features.User.Model.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Shared.Domain.Constants;
using MilkTea.Shared.Extensions;

namespace MilkTea.Application.Features.Auth.Commands;

public class LogoutCommand : IRequest<LogoutResult>
{
    public string RefreshToken { get; set; } = string.Empty;
}

public sealed class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    // Refresh token mà không truyền => Error cũng nên logout luôn
}

public sealed class LogoutCommandHandler(IAuthUnitOfWork authUnitOfWork,
                                            IIdentifyServicePorts currentUser) : IRequestHandler<LogoutCommand, LogoutResult>
{
    private readonly IAuthUnitOfWork _vAuthUnitOfWork = authUnitOfWork;
    private readonly IIdentifyServicePorts _vCurrentUser = currentUser;
    public async Task<LogoutResult> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        var result = new LogoutResult();
        var userId = _vCurrentUser.UserId;
        if (command.RefreshToken.IsNullOrWhiteSpace())
        {
            result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            return result;
        }

        // Load user with tracking for update
        var user = await _vAuthUnitOfWork.Users.GetByIdForUpdateAsync(userId, cancellationToken);
        if (user is null)
        {
            result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            return result;
        }

        await _vAuthUnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // Revoke refresh token
            user.RevokeRefreshToken(command.RefreshToken, userId);

            // Commit transaction 
            await _vAuthUnitOfWork.CommitTransactionAsync(cancellationToken);
            return result;
        }
        catch (Exception)
        {
            // Rollback transaction on error and logout
            await _vAuthUnitOfWork.RollbackTransactionAsync(cancellationToken);
            result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            return result;
        }
    }
}
