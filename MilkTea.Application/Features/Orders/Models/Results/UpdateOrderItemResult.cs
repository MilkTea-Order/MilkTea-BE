using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Models.Results
{
    public class UpdateOrderItemResult
    {
        public StringListEntry ResultData { get; set; } = new();
    }
}
