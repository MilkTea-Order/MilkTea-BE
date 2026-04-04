using MilkTea.API.RestfulAPI.DTOs.Requests;

namespace MilkTea.API.RestfulAPI.DTOs.Auth.Requests
{
    public class LogoutRequestDto : BaseRequestDto
    {
        public string RefreshToken { get; set; } = default!;
    }
}
