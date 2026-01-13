using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Results.Orders
{
    public class CancelOrderDetailsResult
    {
        public int? OrderID { get; set; }
        public List<int> CancelledDetailIDs { get; set; } = new();
        public DateTime? CancelledDate { get; set; }
        public StringListEntry ResultData { get; set; } = new();
    }
}