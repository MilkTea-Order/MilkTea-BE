namespace MilkTea.Application.Commands.Orders
{
    public class CancelOrderCommand
    {
        public int OrderID { get; set; }
        public int CancelledBy { get; set; }
        public string? CancelNote { get; set; }
    }
}