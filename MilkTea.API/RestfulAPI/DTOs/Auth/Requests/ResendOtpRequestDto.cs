using System.ComponentModel.DataAnnotations;

namespace MilkTea.API.RestfulAPI.DTOs.Auth.Requests;

public class ResendOtpRequestDto
{
    [Required]
    public string Channel { get; set; } = string.Empty;
}
