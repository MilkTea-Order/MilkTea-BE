using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Results
{
    public class ProcessPaymentResult
    {
        public StringListEntry ResultData { get; set; } = new();
    }
}
