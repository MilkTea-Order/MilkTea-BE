using System.ComponentModel.DataAnnotations;

namespace MilkTea.API.RestfulAPI.DTOs.Requests
{
    public class UpdatePasswordRequestDto
    {
        [Required]
        public string Password { get; set; } = default!;

        [Required]
        //[MinLength(8)]
        //[MaxLength(50)]
        //[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).+$", ErrorMessage = "NewPassword must contain at least one letter and one digit.")]
        public string NewPassword { get; set; } = default!;

        [Required]
        //[MinLength(8)]
        //[MaxLength(50)]
        //[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).+$", ErrorMessage = "ConfirmNewPassword must contain at least one letter and one digit.")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = default!;

    }
}
