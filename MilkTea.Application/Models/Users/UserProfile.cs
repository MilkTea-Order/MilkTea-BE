namespace MilkTea.Application.Models.Users
{
    public sealed class UserProfile
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public int GenderId { get; set; }
        public string? GenderName { get; set; }
        public string? BirthDay { get; set; }
        public string? IdentityCode { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? CellPhone { get; set; }
        public int PositionId { get; set; }
        public string? PositionName { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public DateTime? StartWorkingDate { get; set; }
        public DateTime? EndWorkingDate { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountName { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankQrCodeBase64 { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}

