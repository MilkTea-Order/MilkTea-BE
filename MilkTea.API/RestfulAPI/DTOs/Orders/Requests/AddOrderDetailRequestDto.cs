namespace MilkTea.API.RestfulAPI.DTOs.Orders.Requests
{
    public class AddOrderDetailRequestDto
    {
        public List<OrderItemRequest> Items { get; set; } = new();
    }
}
