namespace MilkTea.Application.Features.Orders.Models.Dtos
{
    public class ReportOrderDto
    {
        public List<OrderDateGroupDto> DateGroup { get; set; } = new();
        public StaticDto Statics { get; set; } = new();
    }

    public class OrderDateGroupDto
    {
        public DateOnly Date { get; set; }

        public decimal TotalAmount { get; set; }

        public List<OrderDto> Orders { get; set; } = new();
    }
    public class StaticDto
    {
        public decimal TotalAmountCash { get; set; } = 0;
        public decimal TotalAmountShopee { get; set; } = 0;
        public decimal TotalAmountBank { get; set; } = 0;
        public decimal TotalAmountGrab { get; set; } = 0;
        public decimal TotalAmount { get; set; } = 0;
    }
}
