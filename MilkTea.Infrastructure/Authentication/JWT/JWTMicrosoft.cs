using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MilkTea.Application.Ports.Authentication.JWTPorts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MilkTea.Infrastructure.Authentication.JWT
{
    public class JWTMicrosoft(IOptionsSnapshot<JWTOptionDto> jwtOptions) : IJWTServicePort
    {
        private readonly IOptionsSnapshot<JWTOptionDto> _jwtOptions = jwtOptions;
        public (string AccessToken, DateTime ExpiresAt) CreateJwtAccessToken(int userId, Dictionary<string, object>? additionalClaims = null)
        {
            var vClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new("UserID", userId.ToString()),
            };
            var vKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Key));
            var vCreds = new SigningCredentials(vKey, SecurityAlgorithms.HmacSha256);
            var vExpires = DateTime.UtcNow.AddMinutes(_jwtOptions.Value.AccessTokenExpireMinutes);

            var vToken = new JwtSecurityToken(
                issuer: _jwtOptions.Value.Issuer,
                audience: _jwtOptions.Value.Audience,
                claims: vClaims,
                expires: vExpires,
                signingCredentials: vCreds);

            return (new JwtSecurityTokenHandler().WriteToken(vToken), vExpires);
        }

        public (string RefreshToken, DateTime ExpiresAt) CreateJwtRefreshToken()
        {
            var vRandomBytes = new byte[64];
            using var vRng = RandomNumberGenerator.Create();
            vRng.GetBytes(vRandomBytes);
            return (Convert.ToBase64String(vRandomBytes), DateTime.UtcNow.AddMinutes(_jwtOptions.Value.RefreshTokenExpireMinutes));
        }
    }
}
