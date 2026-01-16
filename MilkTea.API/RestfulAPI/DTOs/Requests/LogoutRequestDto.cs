namespace MilkTea.API.RestfulAPI.DTOs.Requests
{
    public class LogoutRequestDto : BaseRequestDto
    {
        public string RefreshToken { get; set; } = default!;
    }
}
