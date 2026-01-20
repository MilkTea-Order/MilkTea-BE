namespace MilkTea.API.RestfulAPI.DTOs.Requests
{
    public class RefreshAccessTokenRequestDto : BaseRequestDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}

