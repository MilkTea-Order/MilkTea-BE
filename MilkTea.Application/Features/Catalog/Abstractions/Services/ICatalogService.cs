using MilkTea.Application.Features.Catalog.Abstractions.Constracts;
using MilkTea.Application.Models.Catalog;


namespace MilkTea.Application.Features.Catalog.Abstractions.Services
{
    public interface ICatalogService
    {
        Task<IReadOnlyDictionary<int, MenuItemDto>> GetMenusAsync(IEnumerable<int> menuIds, CancellationToken cancellationToken = default);

        Task<IReadOnlyDictionary<int, MenuSizeDto>> GetMenuSizesAsync(IEnumerable<int> sizeIds, CancellationToken cancellationToken = default);

        Task<(bool, (int, decimal))> CanPay(int menuId, int sizeId, CancellationToken cancellationToken = default);

        Task<Dictionary<(int MenuID, int SizeID), (bool CanPay, (int PriceListID, decimal Price) Data)>> CanPayBatch(
                                                                        IReadOnlyCollection<(int MenuID, int SizeID)> items,
                                                                        CancellationToken cancellationToken = default);
        Task<bool> TableCanUse(int tableId, CancellationToken cancellationToken = default);

        Task<TableDto?> GetTableAsync(int tableId, CancellationToken cancellationToken = default);

        Task<Dictionary<int, TableDto>> GetTableAsync(IReadOnlyCollection<int> tableIds, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of material recipes for the specified menu order items.
        /// </summary>
        /// <param name="items">A read-only list of menu order items to retrieve recipes for.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing a read-only list of material recipe DTOs.</returns>
        Task<IReadOnlyList<RecipeConstractDto>> GetMenuRecipesAsync(IReadOnlyList<OrderItemsConstractDto> items, CancellationToken cancellationToken);
    }
}
