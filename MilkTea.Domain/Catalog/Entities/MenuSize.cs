using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Catalog.Entities;

/// <summary>
/// Junction entity for Menu-Size relationship with pricing.
/// </summary>
public class MenuSize : Entity<int>
{
    public int MenuID { get; set; }
    public int SizeID { get; set; }
    public decimal? CostPrice { get; set; }
    public decimal? SalePrice { get; set; }

    // Navigations
    public Menu? Menu { get; set; }
    public Size? Size { get; set; }
}
