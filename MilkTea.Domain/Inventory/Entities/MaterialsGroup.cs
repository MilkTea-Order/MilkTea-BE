using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Inventory.Entities;

/// <summary>
/// Materials group entity for categorizing materials.
/// </summary>
public class MaterialsGroup : Entity<int>
{
    public string Name { get; set; } = null!;
    public string? Note { get; set; }

    // Navigation
    public ICollection<Material> Materials { get; set; } = new List<Material>();
}
