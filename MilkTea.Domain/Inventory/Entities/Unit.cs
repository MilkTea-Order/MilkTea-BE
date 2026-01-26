using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Inventory.Entities;

/// <summary>
/// Unit of measure entity.
/// </summary>
public class Unit : Entity<int>
{
    public string Name { get; set; } = null!;
    public string? Abbreviation { get; set; }
}
