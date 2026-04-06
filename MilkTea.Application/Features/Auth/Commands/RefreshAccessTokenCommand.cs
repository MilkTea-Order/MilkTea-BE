using FluentValidation;
using MilkTea.Application.Features.Auth.Models.Results;
using MilkTea.Application.Ports.Authentication.JWTPorts;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Shared.Domain.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Auth.Commands;

public class RefreshAccessTokenCommand : ICommand<RefreshAccessTokenResult>
{
    public string? RefreshToken { get; set; } = string.Empty;
}

public sealed class RefreshAccessTokenCommandValidator : AbstractValidator<RefreshAccessTokenCommand>
{
    public RefreshAccessTokenCommandValidator()
    {
    }
}

public sealed class RefreshAccessTokenCommandHandler(IAuthUnitOfWork authUnitOfWork,
                                                        IJWTServicePort jwtServicePort) : ICommandHandler<RefreshAccessTokenCommand, RefreshAccessTokenResult>
{
    private readonly IAuthUnitOfWork _vAuthUnitOfWork = authUnitOfWork;
    private readonly IJWTServicePort _vJwtServicePort = jwtServicePort;

    public async Task<RefreshAccessTokenResult> Handle(RefreshAccessTokenCommand command, CancellationToken cancellationToken)
    {
        var result = new RefreshAccessTokenResult();

        if (string.IsNullOrWhiteSpace(command.RefreshToken))
        {
            result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            return result;
        }

        // Get valid refresh token through User aggregate repository
        var refreshToken = await _vAuthUnitOfWork.Users.GetValidRefreshTokenByTokenAsync(command.RefreshToken, cancellationToken);

        if (refreshToken is null)
        {
            // Token không tồn tại / đã revoke / hết hạn / không thuộc user
            result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            return result;
        }

        // Check if the token belongs to that person =>  if error => logout => no need to check

        // Create new access token
        var (accessToken, expiresAt) = _vJwtServicePort.CreateJwtAccessToken(refreshToken.UserId);
        result.AccessToken = accessToken;
        result.AccessTokenExpiresAt = expiresAt;

        return result;
    }
}

