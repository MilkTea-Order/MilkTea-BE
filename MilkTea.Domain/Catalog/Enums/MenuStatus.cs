using System.ComponentModel;

namespace MilkTea.Domain.Catalog.Enums;

/// <summary>
/// Status for menu items.
/// Maps to StatusID column in menu table.
/// Uses same values as CommonStatus (Active=1, Inactive=2).
/// </summary>
public enum MenuStatus
{
    [Description("Đang hoạt động")]
    Active = 1,
    [Description("Tạm Ngưng")]
    Inactive = 2
}
