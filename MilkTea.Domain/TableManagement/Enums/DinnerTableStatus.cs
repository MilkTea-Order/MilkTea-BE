namespace MilkTea.Domain.Catalog.Enums;

/// <summary>
/// Status of a dinner table.
/// Maps to StatusOfDinnerTableID column in dinnertable table.
/// </summary>
public enum DinnerTableStatus
{
    Empty = 1,      // Trống
    InUse = 2,      // Đang sử dụng
    Reserved = 3    // Đã đặt
}
