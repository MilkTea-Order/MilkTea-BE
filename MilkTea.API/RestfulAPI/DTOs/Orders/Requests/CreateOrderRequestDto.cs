namespace MilkTea.API.RestfulAPI.DTOs.Orders.Requests
{
    public class CreateOrderRequestDto
    {
        public int DinnerTableID { get; set; }
        public int? OrderByID { get; set; }
        public List<OrderItemRequest> Items { get; set; } = new();
        public string? Note { get; set; }
    }

    public class OrderItemRequest
    {
        public int MenuID { get; set; }
        public int SizeID { get; set; }
        public int Quantity { get; set; }
        public List<int>? ToppingIDs { get; set; }
        public List<int>? KindOfHotpotIDs { get; set; }
        public string? Note { get; set; }
    }
}