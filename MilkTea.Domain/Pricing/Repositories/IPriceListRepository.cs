using MilkTea.Domain.Pricing.Entities;

namespace MilkTea.Domain.Pricing.Repositories;

/// <summary>
/// Repository interface for price list operations.
/// </summary>
public interface IPriceListRepository
{
    /// <summary>
    /// Gets a price list by ID.
    /// </summary>
    Task<PriceList?> GetByIdAsync(int id);

    /// <summary>
    /// Gets the active price list.
    /// </summary>
    Task<PriceList?> GetActivePriceListAsync();

    /// <summary>
    /// Gets a price for a specific menu item and size.
    /// </summary>
    Task<decimal?> GetPriceAsync(int priceListId, int menuId, int sizeId);

    /// <summary>
    /// Gets price list details by price list ID.
    /// </summary>
    Task<List<PriceListDetail>> GetDetailsByPriceListIdAsync(int priceListId);

    /// <summary>
    /// Gets prices for a menu item (all sizes).
    /// </summary>
    Task<Dictionary<int, decimal>> GetPricesForMenuAsync(int priceListId, int menuId, CancellationToken cancellationToken);
}
