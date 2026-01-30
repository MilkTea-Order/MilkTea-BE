namespace MilkTea.API.RestfulAPI.DTOs.Requests
{
    public class EmployeeUpdateProfileRequestDto
    {
        public string? FullName { get; set; } = null!;

        public int? GenderID { get; set; } = null!;

        public string? BirthDay { get; set; } = null!;

        public string? IdentityCode { get; set; } = null!;

        public string? Email { get; set; } = null!;

        public string? Address { get; set; } = null!;

        public string? CellPhone { get; set; } = null!;


        public string? BankName { get; set; } = null!;


        public string? BankAccountName { get; set; } = null!;


        public string? BankAccountNumber { get; set; } = null!;

        public IFormFile? BankQRCode { get; set; } = null!;
    }
}
