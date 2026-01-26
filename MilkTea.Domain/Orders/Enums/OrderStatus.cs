using System.ComponentModel;

namespace MilkTea.Domain.Orders.Enums;

/// <summary>
/// Status of an order.
/// Maps to StatusOfOrderID column in orders table.
/// </summary>
public enum OrderStatus
{
    [Description("Chưa thanh toán")]
    Unpaid = 1,         // Chưa thanh toán
    [Description("Đã thu tiền")]
    Paid = 2,           // Đã thu tiền
    [Description("Hủy")]
    Cancelled = 3,      // Hủy
    [Description("Chưa thu tiền")]
    NotCollected = 4    // Chưa thu tiền
}
