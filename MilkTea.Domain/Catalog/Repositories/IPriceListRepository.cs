using MilkTea.Domain.Catalog.Entities;

namespace MilkTea.Domain.Catalog.Repositories;

/// <summary>
/// Repository interface for price list operations.
/// </summary>
public interface IPriceListRepository
{
    /// <summary>
    ///  Gets a price list by its identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Price List</returns>
    Task<PriceList?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get active price list with details.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Active price list with details.</returns>
    Task<PriceList?> GetActiveWithCurrencyAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the active price list.
    /// </summary>
    /// <returns>Active price list.</returns>
    Task<PriceList?> GetActiveWithRelationshipAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// Gets the active price list with details for a specific menu item and size.
    /// </summary>
    /// <param name="menuId"></param>
    /// <param name="sizeId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Active price list with details for the specified menu and size.</returns>
    Task<PriceList?> GetActiveByMenuAndSizeWithRelationshipAsync(int menuId, int sizeId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Gets active prices for a menu item (all sizes).
    /// </summary>
    /// <param name="menuId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Active price list with details for the specified menu.</returns>
    Task<PriceList?> GetActiveByMenuWithRelationshipAsync(int menuId, CancellationToken cancellationToken);
}
