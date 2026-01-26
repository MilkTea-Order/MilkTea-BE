using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Pricing.Entities;

/// <summary>
/// Price list detail entity for menu item pricing.
/// Child entity of PriceList aggregate.
/// </summary>
public sealed class PriceListDetail : Entity<int>
{
    public int PriceListID { get; private set; }
    public PriceList? PriceList { get; private set; }

    public int MenuID { get; private set; }
    public int SizeID { get; private set; }
    public decimal Price { get; private set; }

    // For EF Core
    private PriceListDetail() { }

    internal static PriceListDetail Create(
        int priceListId,
        int menuId,
        int sizeId,
        decimal price,
        int createdBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(priceListId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(menuId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sizeId);
        ArgumentOutOfRangeException.ThrowIfNegative(price);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        var now = DateTime.UtcNow;

        return new PriceListDetail
        {
            PriceListID = priceListId,
            MenuID = menuId,
            SizeID = sizeId,
            Price = price,
            CreatedBy = createdBy,
            CreatedDate = now
        };
    }

    public void UpdatePrice(decimal price, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(price);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Price = price;
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }
}
