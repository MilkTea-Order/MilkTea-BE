using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Pricing.Entities;

/// <summary>
/// Currency entity.
/// </summary>
public class Currency : EntityId<int>
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
