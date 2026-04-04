using MilkTea.API.RestfulAPI.DTOs.Requests;

namespace MilkTea.API.RestfulAPI.DTOs.Auth.Requests
{
    public class RefreshAccessTokenRequestDto : BaseRequestDto
    {
        public string? RefreshToken { get; set; } = string.Empty;
    }
}

