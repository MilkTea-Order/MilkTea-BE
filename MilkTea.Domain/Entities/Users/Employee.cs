using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Users
{
    [Table("employees")]
    public class Employee : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Column("Code")]
        public string? Code { get; set; }

        [Required, Column("FullName")]
        public string FullName { get; set; } = null!;

        [Required, Column("GenderID")]
        public int GenderID { get; set; }

        [Column("BirthDay")]
        public string? BirthDay { get; set; }

        [Column("IdentityCode")]
        public string? IdentityCode { get; set; }

        [Column("Email")]
        public string? Email { get; set; }

        [Column("Address")]
        public string? Address { get; set; }

        [Column("StartWorkingDate")]
        public DateTime? StartWorkingDate { get; set; }

        [Column("EndWorkingDate")]
        public DateTime? EndWorkingDate { get; set; }

        [Required, Column("PositionID")]
        public int PositionID { get; set; }

        [Column("CreatedBy")]
        public int? CreatedBy { get; set; }

        [Column("CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Column("LastUpdatedBy")]
        public int? LastUpdatedBy { get; set; }

        [Column("LastUpdatedDate")]
        public DateTime? LastUpdatedDate { get; set; }

        [Required, Column("StatusID")]
        public int StatusID { get; set; }

        [Column("CellPhone")]
        public string? CellPhone { get; set; }

        [Column("SalaryByHour")]
        public int? SalaryByHour { get; set; }

        [Column("ShiftFrom")]
        public DateTime? ShiftFrom { get; set; }

        [Column("ShiftTo")]
        public DateTime? ShiftTo { get; set; }

        [Column("CalcSalaryByMinutes")]
        public int? CalcSalaryByMinutes { get; set; }

        [Required, Column("TimekeepingOther")]
        public int TimekeepingOther { get; set; }

        [Column("Bank_AccountNumber")]
        public string? BankAccountNumber { get; set; }

        [Column("Bank_AccountName")]
        public string? BankAccountName { get; set; }

        [Column("Bank_QRCode")]
        public byte[]? BankQRCode { get; set; }

        [Column("BankName")]
        public string? BankName { get; set; }

        [Column("IsBreakTime")]
        public bool? IsBreakTime { get; set; }

        [Column("BreakTimeFrom")]
        public DateTime? BreakTimeFrom { get; set; }

        [Column("BreakTimeTo")]
        public DateTime? BreakTimeTo { get; set; }
    }


}
