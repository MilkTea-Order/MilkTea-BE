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
        public async Task<List<MenuDto>> GetMenusAsync(
    int? groupId,
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

            var query =
                from m in _vContext.Menus.AsNoTracking()
                join g in _vContext.MenuGroups.AsNoTracking() on m.MenuGroupID equals g.Id
                join u in _vContext.Units.AsNoTracking() on m.UnitID equals u.Id
                join p in _vContext.PriceListDetails.AsNoTracking() on m.Id equals p.MenuID
                where m.Status == MenuStatus.Active
                      && g.Status == CommonStatus.Active
                      && p.PriceListID == activePriceListId.Value
                select new
                {
                    m.Id,
                    m.Code,
                    m.Name,
                    m.Note,
                    m.AvatarPicture,
                    MenuGroupId = g.Id,
                    MenuGroupName = g.Name,
                    UnitId = u.Id,
                    UnitName = u.Name,
                    StatusId = (int)m.Status
                };

            if (hasGroup)
                query = query.Where(x => x.MenuGroupId == groupId!.Value);

            if (hasName)
                query = query.Where(x => x.Name != null
                                      && EF.Functions.Like(x.Name, $"%{menuName}%"));

            var rows = await query
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
                MenuGroupName = x.MenuGroupName,
                UnitId = x.UnitId,
                UnitName = x.UnitName,
                StatusId = x.StatusId,
                StatusName = ((MenuStatus)x.StatusId).GetDescription()
            }).ToList();
        }
    }
}

