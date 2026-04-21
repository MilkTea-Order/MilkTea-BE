namespace MilkTea.API.RestfulAPI.DTOs.Auth.Requests
{
    public class VerifyOtpRequestDto
    {
        public string OtpCode { get; set; } = string.Empty;
    }
}
