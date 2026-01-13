namespace MilkTea.Infrastructure.Authentication.JWT
{
    public class JWTOptionDto
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AccessTokenExpireMinutes { get; set; }
        public int RefreshTokenExpireMinutes { get; set; }
    }
}
