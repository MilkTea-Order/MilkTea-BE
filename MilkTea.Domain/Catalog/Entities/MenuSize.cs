namespace MilkTea.Domain.Catalog.Entities;
public sealed class MenuSize
{
    public int MenuID { get; private set; }
    public int SizeID { get; private set; }
    public decimal? CostPrice { get; private set; }
    public decimal? SalePrice { get; private set; }

    private MenuSize() { }

    internal static MenuSize Create(int sizeId, decimal? cost, decimal? sale)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sizeId);
        return new MenuSize { SizeID = sizeId, CostPrice = cost, SalePrice = sale };
    }

    internal void UpdatePrice(decimal? cost, decimal? sale)
    {
        CostPrice = cost;
        SalePrice = sale;
    }
}
