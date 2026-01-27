using MediatR;
using MilkTea.Application.Features.Users.Commands;
using MilkTea.Application.Ports.Authentication.JWTPorts;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Domain.SharedKernel.Repositories;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.Features.Users.Commands;

public sealed class RefreshAccessTokenCommandHandler(
    IUnitOfWork unitOfWork,
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

        // Get valid refresh token through User aggregate repository
        var refreshToken = await unitOfWork.Users.GetValidRefreshTokenByTokenAsync(command.RefreshToken);
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
