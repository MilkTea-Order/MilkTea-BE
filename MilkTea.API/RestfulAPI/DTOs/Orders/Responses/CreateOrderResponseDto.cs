using MilkTea.API.RestfulAPI.DTOs.Orders.Common;

namespace MilkTea.API.RestfulAPI.DTOs.Orders.Responses
{
    public class CreateOrderResponseDto
    {
        public int OrderID { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public DinnerTableUsingDto DinnerTable { get; set; } = null!;
    }
}