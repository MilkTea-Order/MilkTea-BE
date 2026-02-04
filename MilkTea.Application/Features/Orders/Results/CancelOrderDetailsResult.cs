using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Results
{
    public class CancelOrderDetailsResult
    {
        public List<int> CancelledDetailIDs { get; set; } = new();
        public StringListEntry ResultData { get; set; } = new();
    }
}

