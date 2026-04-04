namespace MilkTea.API.RestfulAPI.DTOs.Orders.Responses
{
    public class CancelOrderResponseDto
    {
        public int OrderID { get; set; }
        public string BillNo { get; set; } = string.Empty;
        public DateTime CancelledDate { get; set; }
    }
}