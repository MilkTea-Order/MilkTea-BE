using MilkTea.API.RestfulAPI.DTOs.Requests;

namespace MilkTea.API.RestfulAPI.DTOs.Auth.Requests
{
    public class LoginRequestDto : BaseRequestDto
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
