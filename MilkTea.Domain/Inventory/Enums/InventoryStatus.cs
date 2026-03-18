using System.ComponentModel;

namespace MilkTea.Domain.Inventory.Enums
{
    public enum InventoryStatus
    {
        [Description("Chờ duyệt")]
        InActive = 1,
        [Description("Đã nhập kho")]
        InStock = 2,
    }
}
