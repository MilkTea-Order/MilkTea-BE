using System.ComponentModel;

namespace MilkTea.Domain.Catalog.Menu.Enums;

public enum MenuStatus
{
    [Description("Đang hoạt động")]
    Active = 1,
    [Description("Tạm Ngưng")]
    Inactive = 2
}
