namespace MilkTea.Application.Ports.Catalog;

/// <summary>
/// Query service interface for Catalog module to provide sales-related data to other modules.
/// This abstraction allows Orders module to query Catalog data without direct repository access.
/// </summary>
public interface ICatalogSalesQuery
{
    /// <summary>
    /// Validates table availability, menu status, and quotes unit price for a menu item.
    /// </summary>
    /// <param name="tableId">The dinner table ID to validate.</param>
    /// <param name="menuId">The menu item ID.</param>
    /// <param name="sizeId">The size ID.</param>
    /// <param name="quantity">The quantity of menu.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Validation result with table status, menu status, and unit price.</returns>
    Task<SalesValidationResult> ValidateAndQuoteAsync(int tableId, int menuId, int sizeId, int quantity, CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of sales validation and price quote.
/// </summary>
public class SalesValidationResult
{
    public decimal? UnitPrice { get; set; }
    public int? PriceListId { get; set; }
    public string? ErrorCode { get; set; }
    public string[]? ErrorFields { get; set; }
    public bool IsValid => string.IsNullOrEmpty(ErrorCode) && ErrorFields == null &&
                           UnitPrice.HasValue &&
                           PriceListId.HasValue;
}