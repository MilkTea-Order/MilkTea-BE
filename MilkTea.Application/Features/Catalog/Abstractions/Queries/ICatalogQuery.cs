using MilkTea.Application.Features.Catalog.Models.Dtos.Menu;

namespace MilkTea.Application.Features.Catalog.Abstractions.Queries
{
    public interface ICatalogQuery
    {
        /// <summary>
        /// Asynchronously retrieves a list of menus filtered by group ID and menu name.
        /// </summary>
        /// <param name="groupId">Optional group ID to filter the menus.</param>
        /// <param name="menuName">Optional menu name to filter the menus.</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of MenuDto objects.</returns>
        Task<List<MenuDto>> GetMenusAsync(int? groupId, string? menuName, CancellationToken cancellationToken = default);
    }
}
