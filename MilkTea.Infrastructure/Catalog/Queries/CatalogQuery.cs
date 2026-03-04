using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Catalog.Abstractions;
using MilkTea.Application.Features.Catalog.Dtos;
using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.SharedKernel.Enums;
using MilkTea.Infrastructure.Persistence;
using Shared.Extensions;

namespace MilkTea.Infrastructure.Catalog.Queries
{
    public class CatalogQuery(AppDbContext context) : ICatalogQuery
    {
        private readonly AppDbContext _vContext = context;

        ///<inheritdoc/>
        public async Task<List<MenuDto>> GetMenusAsync(int? groupId,
                                                        string? menuName,
                                                        CancellationToken cancellationToken = default)
        {
            var hasGroup = groupId.HasValue;
            var hasName = !string.IsNullOrWhiteSpace(menuName);

            if (!hasGroup && !hasName) return new();

            var activePriceListId = await _vContext.PriceLists.AsNoTracking()
                                                                .Where(x => x.Status == PriceListStatus.Active)
                                                                .Select(x => (int?)x.Id)
                                                                .FirstOrDefaultAsync(cancellationToken);

            if (activePriceListId is null) return new();

            var rows = await (
                from m in _vContext.Menus.AsNoTracking()
                join u in _vContext.Units.AsNoTracking() on m.UnitID equals u.Id
                where m.Status == MenuStatus.Active
                      // Groups must be active
                      && _vContext.MenuGroups.AsNoTracking()
                            .Any(g => g.Id == m.MenuGroupID && g.Status == CommonStatus.Active)

                      // Filter optional
                      && (!hasGroup || m.MenuGroupID == groupId!.Value)
                      && (!hasName || (m.Name != null && EF.Functions.Like(m.Name, $"%{menuName}%")))

                      // menu phải có giá trong pricelist active
                      && _vContext.PriceListDetails.AsNoTracking()
                            .Any(p => p.MenuID == m.Id && p.PriceListID == activePriceListId.Value)

                select new
                {
                    m.Id,
                    m.Code,
                    m.Name,
                    m.Note,
                    m.AvatarPicture,
                    MenuGroupId = m.MenuGroupID,
                    //MenuGroupName = g.Name,
                    UnitId = u.Id,
                    UnitName = u.Name,
                    StatusId = (int)m.Status
                }
            )
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

            return rows.Select(x => new MenuDto
            {
                MenuId = x.Id,
                MenuCode = x.Code,
                MenuName = x.Name,
                Note = x.Note,
                MenuImage = x.AvatarPicture != null
                    ? $"data:image/png;base64,{Convert.ToBase64String(x.AvatarPicture)}"
                    : null,
                MenuGroupId = x.MenuGroupId,
                //MenuGroupName = x.MenuGroupName,
                UnitId = x.UnitId,
                UnitName = x.UnitName,
                StatusId = x.StatusId,
                StatusName = ((MenuStatus)x.StatusId).GetDescription()
            }).ToList();
        }


    }
}

