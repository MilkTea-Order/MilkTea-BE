namespace MilkTea.API.RestfulAPI.DTOs.Responses
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserResponseDto User { get; set; } = new();
        public List<Dictionary<string, object?>> Permissions { get; set; } = new();
    }
}
