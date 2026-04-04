using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Domain.Catalog.Menu.Entities;

public class MenuMaterialRecipeEntity : EntityId<int>
{
    public int MenuID { get; set; }
    public int SizeID { get; set; }
    public int MaterialID { get; set; }
    public decimal Quantity { get; set; }
}
