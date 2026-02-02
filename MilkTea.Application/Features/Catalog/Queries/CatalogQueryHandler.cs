using MilkTea.Application.Models.Catalog;
using MilkTea.Application.Ports.Catalog;
using MilkTea.Domain.Catalog.Repositories;

namespace MilkTea.Application.Features.Catalog.Queries;

/// <summary>
/// Handler for catalog sales queries.
/// Implements ICatalogSalesQuery to provide sales validation and pricing to other modules.
/// </summary>
public class CatalogQueryHandler(ICatalogUnitOfWork catalogUnitOfWork) : ICatalogQuery
{
    private readonly ICatalogUnitOfWork _vCatalogUnitOfWork = catalogUnitOfWork;
    public async Task<SalesValidationResult> ValidateAndQuoteAsync(int tableId, int menuId, int sizeId, int quantity, CancellationToken cancellationToken = default)
    {
        var result = new SalesValidationResult();

        // Validate table (Not Found or Not Empty)
        var emptyTables = await _vCatalogUnitOfWork.Tables.GetTableEmptyAsync(true, cancellationToken);
        var isTableEmpty = emptyTables.Any(t => t.Id == tableId);
        //Console.WriteLine($"isTableEmpty: {isTableEmpty}");
        if (!isTableEmpty)
        {
            return Error(result, "TABLE", [$"{tableId}"]);
        }
        if (quantity <= 0)
        {
            return Error(result, "QUANTITY", [$"{menuId}", $"{sizeId}"]);
        }

        // Validate Menu and Size (Active)
        var isMenuSizeActive = await _vCatalogUnitOfWork.Menus.isActiceMenuAndSize(menuId, sizeId, cancellationToken);
        //Console.WriteLine($"isMenuSizeActive: {isMenuSizeActive}");
        if (!isMenuSizeActive)
        {
            return Error(result, "MENU_SIZE", [$"{menuId}", $"{sizeId}"]);
        }

        // Price
        var priceList = await _vCatalogUnitOfWork.PriceLists.GetActiveByMenuAndSizeWithRelationshipAsync(menuId, sizeId, cancellationToken);
        var priceDetail = priceList?.Details?.FirstOrDefault();
        if (priceDetail is null)
        {
            return Error(result, "PRICE", [$"{menuId}", $"{sizeId}"]);
        }
        result.UnitPrice = priceDetail.Price;
        result.PriceListId = priceList!.Id;
        return result;
    }

    private static SalesValidationResult Error(SalesValidationResult r, string code, string[] reason)
    {
        r.ErrorCode = code;
        r.ErrorFields = reason;
        return r;
    }

    public Task<IReadOnlyDictionary<int, MenuItemDto>> GetMenusAsync(IEnumerable<int> menuIds, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyDictionary<int, MenuSizeDto>> GetMenuSizesAsync(IEnumerable<int> sizeIds, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
