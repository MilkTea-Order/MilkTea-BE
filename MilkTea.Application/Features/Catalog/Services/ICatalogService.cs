using MilkTea.Application.Models.Catalog;

namespace MilkTea.Application.Features.Catalog.Services
{
    public interface ICatalogService
    {
        Task<IReadOnlyDictionary<int, MenuItemDto>> GetMenusAsync(IEnumerable<int> menuIds, CancellationToken cancellationToken = default);

        Task<IReadOnlyDictionary<int, MenuSizeDto>> GetMenuSizesAsync(IEnumerable<int> sizeIds, CancellationToken cancellationToken = default);

        Task<TableDto?> GetTableAsync(int tableId, CancellationToken cancellationToken = default);
    }
}
