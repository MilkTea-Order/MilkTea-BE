using System.ComponentModel.DataAnnotations;

namespace MilkTea.API.RestfulAPI.DTOs.Auth.Requests
{
    public class ResetPasswordRequestDto
    {
        [Required]
        public string ResetPasswordToken { get; set; } = default!;

        [Required]
        public string NewPassword { get; set; } = default!;

        [Required]
        public string ConfirmPassword { get; set; } = default!;
    }
}
