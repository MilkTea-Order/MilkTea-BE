using System.ComponentModel.DataAnnotations;

namespace MilkTea.API.RestfulAPI.DTOs.Requests
{
    public class AdminUpdateUserRequestDto
    {

        [StringLength(150)]
        public string? FullName { get; set; }

        [Range(1, int.MaxValue)]
        public int? GenderID { get; set; }

        [StringLength(20)]
        public string? BirthDay { get; set; }

        [StringLength(50)]
        public string? IdentityCode { get; set; }

        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        [Phone]
        [StringLength(20)]
        public string? CellPhone { get; set; }

        [Range(1, int.MaxValue)]
        public int? PositionID { get; set; }

        public DateTime? StartWorkingDate { get; set; }
        public DateTime? EndWorkingDate { get; set; }

        [Range(1, int.MaxValue)]
        public int? StatusID { get; set; }


        [Range(0, int.MaxValue)]
        public int? SalaryByHour { get; set; }

        [Range(0, int.MaxValue)]
        public int? CalcSalaryByMinutes { get; set; }

        public DateTime? ShiftFrom { get; set; }
        public DateTime? ShiftTo { get; set; }

        public bool? IsBreakTime { get; set; }
        public DateTime? BreakTimeFrom { get; set; }
        public DateTime? BreakTimeTo { get; set; }

        [StringLength(100)]
        public string? BankName { get; set; }

        [StringLength(100)]
        public string? BankAccountName { get; set; }

        [StringLength(50)]
        public string? BankAccountNumber { get; set; }

        public byte[]? BankQRCode { get; set; }
    }
}
