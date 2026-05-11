using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Models.Results;

public class UpdateOrderItemStatusResult
{
    public StringListEntry ResultData { get; set; } = new();
}
