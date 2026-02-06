namespace MilkTea.API.RestfulAPI.DTOs.Orders.Requests
{
    public class UpdateOrderDetailRequestDto
    {
        public int? Quantity { get; set; }
        public string? Note { get; set; }
    }
}
