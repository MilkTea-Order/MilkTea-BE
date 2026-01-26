using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Results
{
    public class CancelOrderResult
    {
        public int? OrderID { get; set; }
        public string? BillNo { get; set; }
        public DateTime? CancelledDate { get; set; }

        public StringListEntry ResultData { get; set; } = new();
    }
}

