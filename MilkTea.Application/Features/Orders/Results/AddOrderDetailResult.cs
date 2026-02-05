using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Results
{
    public class AddOrderDetailResult
    {
        public StringListEntry ResultData { get; set; } = new();
    }
}
