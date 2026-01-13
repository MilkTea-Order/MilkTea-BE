using System.ComponentModel.DataAnnotations;

namespace MilkTea.API.RestfulAPI.DTOs.Requests
{
    public class EmployeeUpdateProfileRequestDto
    {
        [StringLength(150, MinimumLength = 2, ErrorMessage = "FullName length must be between 2 and 150")]
        public string? FullName { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "GenderID must be greater than 0")]
        public int? GenderID { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }

        [RegularExpression(@"^\d{9}|\d{12}$", ErrorMessage = "IdentityCode must be 9 or 12 digits")]
        [StringLength(12)]
        public string? IdentityCode { get; set; }

        [EmailAddress(ErrorMessage = "Email is invalid")]
        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        [Phone(ErrorMessage = "CellPhone is invalid")]
        [StringLength(12, MinimumLength = 9, ErrorMessage = "Phone length must be between 9 and 12")]
        public string? CellPhone { get; set; }

        [StringLength(100)]
        public string? BankName { get; set; }

        [StringLength(100)]
        public string? BankAccountName { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "BankAccountNumber must contain digits only")]
        public string? BankAccountNumber { get; set; }

        public byte[]? BankQRCode { get; set; }
    }
}
