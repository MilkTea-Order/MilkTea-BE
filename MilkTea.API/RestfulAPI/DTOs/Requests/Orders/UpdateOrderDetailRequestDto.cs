namespace MilkTea.API.RestfulAPI.DTOs.Requests.Orders
{
    public class UpdateOrderDetailRequestDto
    {
        public int? Quantity { get; set; }
        public string? Note { get; set; }
    }
}
