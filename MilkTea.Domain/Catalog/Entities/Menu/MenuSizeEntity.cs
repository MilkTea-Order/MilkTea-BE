namespace MilkTea.Domain.Catalog.Entities.Menu;
public sealed class MenuSizeEntity
{
    public int MenuID { get; private set; }
    public int SizeID { get; private set; }
    public decimal? CostPrice { get; private set; }
    public decimal? SalePrice { get; private set; }

    private MenuSizeEntity() { }

    internal static MenuSizeEntity Create(int sizeId, decimal? cost, decimal? sale)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sizeId);
        return new MenuSizeEntity { SizeID = sizeId, CostPrice = cost, SalePrice = sale };
    }

    internal void UpdatePrice(decimal? cost, decimal? sale)
    {
        CostPrice = cost;
        SalePrice = sale;
    }
}
