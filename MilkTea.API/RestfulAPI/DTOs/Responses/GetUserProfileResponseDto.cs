namespace MilkTea.API.RestfulAPI.DTOs.Responses
{
    public class GetUserProfileResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public int EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string FullName { get; set; } = null!;
        public int GenderID { get; set; }
        public string? GenderName { get; set; }
        public string? BirthDay { get; set; }
        public string? IdentityCode { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? CellPhone { get; set; }
        public int PositionID { get; set; }
        public string? PositionName { get; set; }
        public int StatusID { get; set; }
        public string? StatusName { get; set; }
        public DateTime? StartWorkingDate { get; set; }
        public DateTime? EndWorkingDate { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountName { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankQRCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
