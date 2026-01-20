using MilkTea.Application.Commands.Users;
using MilkTea.Application.Results.Users;
using MilkTea.Application.Ports.Authentication.JWTPort;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.UseCases.Users
{
    public class RefreshAccessTokenUseCase(IRefreshTokenRepository refreshTokenRepository,
                                           IJWTServicePort jwtServicePort)
    {
        private readonly IRefreshTokenRepository _vRefreshTokenRepository = refreshTokenRepository;
        private readonly IJWTServicePort _vJwtServicePort = jwtServicePort;

        public async Task<RefreshAccessTokenResult> Execute(RefreshAccessTokenCommand command)
        {
            RefreshAccessTokenResult result = new();

            // Lấy refresh token hợp lệ
            var refreshToken = await _vRefreshTokenRepository.GetValidTokenByTokenAsync(command.RefreshToken);
            if (refreshToken == null )
            {
                // Token không tồn tại / đã revoke / hết hạn / không thuộc user
                result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
                return result;
            }

            // Tạo access token mới cho user
            (var accessToken, var expiresAt) = _vJwtServicePort.CreateJwtAccessToken(refreshToken.UserId);
            result.AccessToken = accessToken;
            result.AccessTokenExpiresAt = expiresAt;

            return result;
        }
    }
}

