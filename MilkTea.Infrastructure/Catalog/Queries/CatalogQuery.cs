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
        public async Task<List<MenuDto>> GetMenusAsync(int? groupId, string? menuName, CancellationToken cancellationToken = default)
        {

            var hasGroup = groupId.HasValue;
            var hasName = !string.IsNullOrWhiteSpace(menuName);

            if (!hasGroup && !hasName) return new();

            var activePriceListId = await _vContext.PriceLists.AsNoTracking()
                .Where(x => x.Status == PriceListStatus.Active)
                .Select(x => (int?)x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            var query =
               from m in _vContext.Menus.AsNoTracking()
               join g in _vContext.MenuGroups.AsNoTracking()
                   on m.MenuGroupID equals g.Id
               join u in _vContext.Units.AsNoTracking()
                   on m.UnitID equals u.Id
               where m.Status == MenuStatus.Active
                     && g.Status == CommonStatus.Active
               select new MenuDto
               {
                   MenuId = m.Id,
                   MenuCode = m.Code,
                   MenuName = m.Name,
                   Note = m.Note,

                   MenuGroupId = g.Id,
                   MenuGroupName = g.Name,

                   UnitId = u.Id,
                   UnitName = u.Name,

                   StatusId = (int)m.Status,
                   StatusName = m.Status.GetDescription()
               };

            if (hasGroup)
                query = query.Where(x => x.MenuGroupId == groupId!.Value);

            if (hasName)
                query = query.Where(x => EF.Functions.Like(x.MenuName!, $"%{menuName}%"));

            return await query
                .OrderBy(x => x.MenuId)
                .Select(x => new MenuDto
                {
                    MenuId = x.MenuId,
                    MenuCode = x.MenuCode,
                    MenuName = x.MenuName,

                    MenuGroupId = x.MenuGroupId,
                    MenuGroupName = x.MenuGroupName,

                    StatusId = x.StatusId,
                    StatusName = x.StatusName,

                    UnitId = x.UnitId,
                    UnitName = x.UnitName,

                    Note = x.Note,

                })
                .ToListAsync(cancellationToken);
        }
    }
}

