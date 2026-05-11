using MilkTea.Application.Features.Orders.Models.Dtos;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Models.Results;

public sealed class GetKitchenOrdersResult
{
    public StringListEntry ResultData { get; set; } = new();
    public List<KitchenOrderDto> Orders { get; set; } = new();
}
