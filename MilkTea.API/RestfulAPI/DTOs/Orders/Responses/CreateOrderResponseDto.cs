using MilkTea.API.RestfulAPI.DTOs.Common;
using MilkTea.API.RestfulAPI.DTOs.Orders.Common;

namespace MilkTea.API.RestfulAPI.DTOs.Orders.Responses
{
    public class CreateOrderResponseDto
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string? Note { get; set; }
        public decimal TotalAmount { get; set; }
        public DinnerTableUsingDto? DinnerTable { get; set; }
        public StatusBaseDto? Status { get; set; }
    }
}