using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Results
{
    public class CreateOrderResult
    {
        public int? OrderID { get; set; }
        public string? BillNo { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? OrderDate { get; set; }
        //public List<OrderItemResult> Items { get; set; } = new();
        public StringListEntry ResultData { get; set; } = new();
    }

    //public class OrderItemResult
    //{
    //    public int MenuID { get; set; }
    //    public string? MenuName { get; set; }
    //    public int SizeID { get; set; }
    //    public string? SizeName { get; set; }
    //    public int Quantity { get; set; }
    //    public decimal Price { get; set; }
    //    public decimal TotalPrice { get; set; }
    //}
}

