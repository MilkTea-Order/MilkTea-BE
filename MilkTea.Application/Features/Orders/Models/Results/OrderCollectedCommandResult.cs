using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Models.Results
{
    public class OrderCollectedCommandResult
    {
        public StringListEntry ResultData { get; set; } = new();
    }
}
