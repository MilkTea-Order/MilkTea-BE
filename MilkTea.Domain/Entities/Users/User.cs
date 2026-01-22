namespace MilkTea.Domain.Entities.Users
{
    public class User : BaseModel
    {
        public int ID { get; set; }
        public int EmployeesID { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? StoppedBy { get; set; }
        public DateTime? StoppedDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int? PasswordResetBy { get; set; }
        public DateTime? PasswordResetDate { get; set; }
        public int StatusID { get; set; }

        // ===== Navigations =====
        public Employee? Employee { get; set; }
        public Status? Status { get; set; }
    }
}
