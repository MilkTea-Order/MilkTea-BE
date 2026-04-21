namespace MilkTea.API.RestfulAPI.DTOs.Auth.Requests
{
    public class CreateOtpRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Function { get; set; } = string.Empty;
    }
}
