using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Users
{
    [Table("users")]
    public class User : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Required, Column("EmployeesID")]
        public int EmployeesID { get; set; }

        [Required, Column("UserName")]
        public string UserName { get; set; } = null!;

        [Required, Column("Password")]
        public string Password { get; set; } = null!;

        [Column("CreatedBy")]
        public int? CreatedBy { get; set; }

        [Column("CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Column("StoppedBy")]
        public int? StoppedBy { get; set; }

        [Column("StoppedDate")]
        public DateTime? StoppedDate { get; set; }

        [Column("LastUpdatedBy")]
        public int? LastUpdatedBy { get; set; }

        [Column("LastUpdatedDate")]
        public DateTime? LastUpdatedDate { get; set; }

        [Column("Password_ResetBy")]
        public int? PasswordResetBy { get; set; }

        [Column("Password_ResetDate")]
        public DateTime? PasswordResetDate { get; set; }

        [Required, Column("StatusID")]
        public int StatusID { get; set; }

        // ===== Navigations =====
        public Employee? Employee { get; set; }
        public Status? Status { get; set; }
    }

}
