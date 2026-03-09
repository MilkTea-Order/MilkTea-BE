using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Models.Results
{
    public class UpdateOrderDetailResult
    {
        public StringListEntry ResultData { get; set; } = new();
    }
}
