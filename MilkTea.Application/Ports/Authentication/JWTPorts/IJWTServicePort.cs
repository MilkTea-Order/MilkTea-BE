namespace MilkTea.Application.Ports.Authentication.JWTPorts
{
    public interface IJWTServicePort
    {
        public (string AccessToken, DateTime ExpiresAt) CreateJwtAccessToken(int userId, Dictionary<string, object>? additionalClaims = null);
        public (string RefreshToken, DateTime ExpiresAt) CreateJwtRefreshToken();
    }
}
