using MilkTea.Application.Features.Catalog.Models.Dtos;
using MilkTea.Application.Features.Catalog.Models.Dtos.Menu;
using MilkTea.Domain.Common.Enums;

namespace MilkTea.Application.Features.Catalog.Abstractions.Queries
{
    public interface ICatalogQuery
    {
        Task<List<MenuDto>> GetMenusAsync(
            int? groupId,
            string? name,
            CommonStatus menuStatus = CommonStatus.Active,
            CommonStatus groupStatus = CommonStatus.Active,
            CancellationToken cancellationToken = default);

        Task<List<MenuGroupDto>> GetGroupMenuAvailableAsync(CancellationToken cancellationToken = default);

        Task<List<SizeAndPriceDto>> GetMenuSizesAsync(int menuId, CancellationToken cancellationToken = default);
    }
}
