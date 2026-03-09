using System.ComponentModel;

namespace MilkTea.Domain.Catalog.Material.Enums;

public enum MaterialStatus
{
    [Description("Đang sử dụng")]
    Active = 1,
    [Description("Tạm Ngưng")]
    Inactive = 2
}
