namespace MilkTea.API.RestfulAPI.DTOs.Requests.Orders
{
    public class AddOrderDetailRequestDto
    {
        public List<OrderItemRequest> Items { get; set; } = new();
    }
}
