namespace MilkTea.API.RestfulAPI.DTOs.Responses
{
    public class CancelOrderDetailsResponseDto
    {
        public int OrderID { get; set; }
        public List<int> CancelledDetailIDs { get; set; } = new();
        public DateTime CancelledDate { get; set; }
    }
}