namespace MilkTea.API.RestfulAPI.DTOs.Orders.Requests
{
    public class CancelOrderDetailsRequestDto
    {
        public List<int> OrderDetailIDs { get; set; } = new();
    }
}