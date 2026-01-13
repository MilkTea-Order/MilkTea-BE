namespace MilkTea.Application.Commands.Users
{
    public class EmployeeUpdateProfileCommand
    {
        public int UserID { get; set; }

        public string? FullName { get; set; } = null!;
        public int? GenderID { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? IdentityCode { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? CellPhone { get; set; }

        public string? BankName { get; set; }
        public string? BankAccountName { get; set; }
        public string? BankAccountNumber { get; set; }
        public byte[]? BankQRCode { get; set; }
    }
}
