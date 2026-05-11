using System.ComponentModel;

namespace MilkTea.Domain.Orders.Enums;

public enum OrderStatus
{
    [Description("Chưa thanh toán")]
    Unpaid = 1,
    [Description("Đã thu tiền")]
    Paid = 2,
    [Description("Hủy")]
    Cancelled = 3,
    [Description("Chưa thu tiền")]
    NotCollected = 4
}
