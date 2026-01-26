using System.ComponentModel;

namespace MilkTea.Domain.SharedKernel.Enums;

/// <summary>
/// Common status for entities that have Active/Inactive state.
/// Maps to StatusID column in database.
/// </summary>
public enum CommonStatus
{
    [Description("Đang hoạt động")]
    Active = 1,

    [Description("Tạm ngừng")]
    Inactive = 2
}
