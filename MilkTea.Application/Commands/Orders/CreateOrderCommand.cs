namespace MilkTea.Application.Commands.Orders
{
    public class CreateOrderCommand
    {
        public int DinnerTableID { get; set; }
        public List<OrderItemCommand> Items { get; set; } = new();
        public int OrderedBy { get; set; }
        public int CreatedBy { get; set; }
        public string? Note { get; set; }
    }



    public class OrderItemCommand
    {
        public int MenuID { get; set; }
        public int SizeID { get; set; }
        public int Quantity { get; set; }
        public List<int>? ToppingIDs { get; set; }
        public List<int>? KindOfHotpotIDs { get; set; }
        public string? Note { get; set; }
    }
}