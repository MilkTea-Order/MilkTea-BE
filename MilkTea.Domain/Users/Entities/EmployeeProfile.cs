using MilkTea.Domain.Users.Enums;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Users.Entities;

/// <summary>
/// Employee profile entity.
/// Maps to employees table.
/// </summary>
public class EmployeeProfile : Entity<int>
{
    public string? Code { get; set; }
    public string FullName { get; set; } = null!;
    public int GenderID { get; set; }
    public string? BirthDay { get; set; }
    public string? IdentityCode { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateTime? StartWorkingDate { get; set; }
    public DateTime? EndWorkingDate { get; set; }
    public int PositionID { get; set; }
    public new int? CreatedBy { get; set; }
    public new DateTime? CreatedDate { get; set; }
    public int? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    
    /// <summary>
    /// Employee status. Maps to StatusID column.
    /// </summary>
    public UserStatus Status { get; set; }
    
    public string? CellPhone { get; set; }
    public int? SalaryByHour { get; set; }
    public DateTime? ShiftFrom { get; set; }
    public DateTime? ShiftTo { get; set; }
    public int? CalcSalaryByMinutes { get; set; }
    public int TimekeepingOther { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? BankAccountName { get; set; }
    public byte[]? BankQRCode { get; set; }
    public string? BankName { get; set; }
    public bool? IsBreakTime { get; set; }
    public DateTime? BreakTimeFrom { get; set; }
    public DateTime? BreakTimeTo { get; set; }

    // Navigations
    public Gender? Gender { get; set; }
    public Position? Position { get; set; }
    public User? User { get; set; }
}
