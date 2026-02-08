using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Inventory.Entities;

/// <summary>
/// Junction entity linking menu items to required materials (recipe).
/// Maps to menuandmaterial table.
/// </summary>
public class MenuMaterialRecipe : Entity<int>
{
    public int MenuID { get; set; }
    public int MaterialID { get; set; }
    public decimal Quantity { get; set; }
    public int? UnitID { get; set; }
    // Navigations
    public Material? Material { get; set; }
}
