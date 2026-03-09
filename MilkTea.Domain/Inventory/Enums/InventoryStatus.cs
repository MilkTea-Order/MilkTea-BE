using System.ComponentModel;

namespace MilkTea.Domain.Inventory.Enums
{
    public enum InventoryStatus
    {
        [Description("Đang chờ duyệt")]
        InActive = 1,
        [Description("Còn hàng")]
        InStock = 2,
    }
}
