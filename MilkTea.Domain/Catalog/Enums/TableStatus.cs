using System.ComponentModel;

namespace MilkTea.Domain.Catalog.Enums;

/// <summary>
/// Status of a dinner table.
/// Maps to StatusOfDinnerTableID column in dinnertable table.
/// </summary>
public enum TableStatus
{
    [Description("Đang sử dụng")]
    InUsing = 1,      // Đang sử dụng
    [Description("Đang sửa chữa")]
    InRepairing = 2,      // Đang sử dụng
    [Description("Ngừng phục vụ")]
    IsOutOfService = 3    // Ngừng phục vụ
}
