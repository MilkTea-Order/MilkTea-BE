using System.ComponentModel;

namespace MilkTea.Domain.Orders.Enums;

public enum OrderItemStatus
{
    [Description("Chờ thực hiện")]
    Pending = 1,

    [Description("Đang thực hiện")]
    InProgress = 2,

    [Description("Đã hoàn thành")]
    Completed = 3,

    [Description("Đã hủy")]
    Cancelled = 4
}

public static class OrderDetailStatusExtensions
{
    public static bool TryParse(string? value, out OrderItemStatus status)
    {
        // status = default;
        // if (string.IsNullOrWhiteSpace(value))
        //     return false;
        //
        // if (Enum.TryParse(value, ignoreCase: true, out status))
        //     return true;
        //
        // status = default;
        // return false;
        status = default;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        return Enum.TryParse(value, true, out status);
    }
}
