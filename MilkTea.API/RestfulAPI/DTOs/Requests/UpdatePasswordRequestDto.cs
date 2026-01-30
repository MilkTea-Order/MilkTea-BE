using System.ComponentModel.DataAnnotations;

namespace MilkTea.API.RestfulAPI.DTOs.Requests
{
    public class UpdatePasswordRequestDto
    {
        [Required]
        public string Password { get; set; } = default!;

        [Required]
        public string NewPassword { get; set; } = default!;

        [Required]
        public string ConfirmPassword { get; set; } = default!;

    }
}
