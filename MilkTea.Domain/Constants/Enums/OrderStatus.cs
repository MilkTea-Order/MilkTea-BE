namespace MilkTea.Domain.Constants.Enums
{
    public enum OrderStatus
    {
        Unpaid = 1, // Chưa thanh toán
        Paid = 2, // Đã thu tiền
        Cancelled = 3, // Hủy
        NotCollected = 4  // Chưa thu tiền
    }
}
