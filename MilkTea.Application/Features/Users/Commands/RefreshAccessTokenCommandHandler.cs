using MediatR;
using MilkTea.Application.Features.Users.Commands;
using MilkTea.Application.Ports.Authentication.JWTPorts;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.Features.Users.Commands;

public sealed class RefreshAccessTokenCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IJWTServicePort jwtServicePort) : IRequestHandler<RefreshAccessTokenCommand, RefreshAccessTokenResult>
{
    public async Task<RefreshAccessTokenResult> Handle(RefreshAccessTokenCommand command, CancellationToken cancellationToken)
    {
        var result = new RefreshAccessTokenResult();

        if (string.IsNullOrWhiteSpace(command.RefreshToken))
        {
            result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            return result;
        }

        // Get valid refresh token
        var refreshToken = await refreshTokenRepository.GetValidTokenByTokenAsync(command.RefreshToken);
        if (refreshToken is null)
        {
            // Token không tồn tại / đã revoke / hết hạn / không thuộc user
            result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            return result;
        }

        // Create new access token
        var (accessToken, expiresAt) = jwtServicePort.CreateJwtAccessToken(refreshToken.UserId);
        result.AccessToken = accessToken;
        result.AccessTokenExpiresAt = expiresAt;

        return result;
    }
}
