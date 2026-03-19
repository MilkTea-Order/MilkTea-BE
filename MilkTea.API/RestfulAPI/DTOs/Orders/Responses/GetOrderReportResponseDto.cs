using MilkTea.API.RestfulAPI.DTOs.Orders.Common;

namespace MilkTea.API.RestfulAPI.DTOs.Orders.Responses
{
    public class GetOrderReportResponseDto
    {
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
        public StaticOrderReportResponseDto Statics { get; set; } = new StaticOrderReportResponseDto();
    }

    public class StaticOrderReportResponseDto
    {
        public decimal TotalAmountCash { get; set; } = 0;
        public decimal TotalAmountShopee { get; set; } = 0;
        public decimal TotalAmountBank { get; set; } = 0;
        public decimal TotalAmountGrab { get; set; } = 0;
        public decimal TotalAmount { get; set; } = 0;
    }
}
