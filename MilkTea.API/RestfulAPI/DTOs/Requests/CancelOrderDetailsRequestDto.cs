namespace MilkTea.API.RestfulAPI.DTOs.Requests
{
    public class CancelOrderDetailsRequestDto
    {
        public List<int> OrderDetailIDs { get; set; } = new();
    }
}