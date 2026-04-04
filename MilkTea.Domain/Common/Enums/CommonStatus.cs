using System.ComponentModel;

namespace MilkTea.Domain.Common.Enums;

public enum CommonStatus
{
    [Description("Đang hoạt động")]
    Active = 1,

    [Description("Tạm ngừng")]
    Inactive = 2
}
