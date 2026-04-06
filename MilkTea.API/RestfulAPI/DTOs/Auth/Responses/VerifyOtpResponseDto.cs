namespace MilkTea.API.RestfulAPI.DTOs.Auth.Responses;

public class VerifyOtpResponseDto
{
    public string ResetPasswordToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
