using MilkTea.Application.Features.Catalog.Dtos;

namespace MilkTea.Application.Features.Catalog.Abstractions
{
    public interface ICatalogQuery
    {
        Task<List<MenuDto>> GetMenusAsync(int? groupId, string? menuName, CancellationToken cancellationToken = default);
    }
}
