namespace MilkTea.Application.Commands.Orders
{
    public class CancelOrderDetailsCommand
    {
        public int OrderID { get; set; }
        public List<int> OrderDetailIDs { get; set; } = new();
        public int CancelledBy { get; set; }
    }
}