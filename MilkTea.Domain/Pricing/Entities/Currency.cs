using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Pricing.Entities;

/// <summary>
/// Currency entity.
/// </summary>
public class Currency : Entity<int>
{
    public string Name { get; set; } = null!;
    public string? Symbol { get; set; }
    public string? Code { get; set; }
}
