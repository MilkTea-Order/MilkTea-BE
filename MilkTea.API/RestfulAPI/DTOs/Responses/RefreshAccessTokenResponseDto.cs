namespace MilkTea.API.RestfulAPI.DTOs.Responses
{
    public class RefreshAccessTokenResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}

