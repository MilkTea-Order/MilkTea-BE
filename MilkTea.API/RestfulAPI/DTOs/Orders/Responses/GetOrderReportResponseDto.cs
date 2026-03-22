using MilkTea.API.RestfulAPI.DTOs.Orders.Common;

namespace MilkTea.API.RestfulAPI.DTOs.Orders.Responses
{
    public class GetOrderReportResponseDto
    {
        public List<DateGroupOrderReportResponseDto> Dates { get; set; } = new List<DateGroupOrderReportResponseDto>();
        public StaticOrderReportResponseDto Statics { get; set; } = new StaticOrderReportResponseDto();
    }

    public class DateGroupOrderReportResponseDto
    {
        public DateOnly Date { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
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
