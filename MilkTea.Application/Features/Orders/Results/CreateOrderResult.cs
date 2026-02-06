using MilkTea.Application.Models.Orders;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Results
{
    public class CreateOrderResult
    {
        public int? OrderID { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? OrderDate { get; set; }
        public Table? DinnerTable { get; set; }
        public StringListEntry ResultData { get; set; } = new();
    }
}

