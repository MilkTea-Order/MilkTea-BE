using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Catalog.Abstractions.Constracts;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Catalog.Models.Dtos;
using MilkTea.Application.Features.Catalog.Models.Dtos.Menu;
using MilkTea.Application.Features.Catalog.Models.Dtos.Size;
using MilkTea.Domain.Catalog.Price.Enums;
using MilkTea.Domain.Catalog.Table.Enums;
using MilkTea.Domain.Common.Enums;
using MilkTea.Infrastructure.Persistence;
using Shared.Extensions;
using TableDto = MilkTea.Application.Features.Catalog.Models.Dtos.Table.TableDto;

namespace MilkTea.Infrastructure.Features.Catalog.Services
{
    public class CatalogService(AppDbContext context) : ICatalogService
    {
        private readonly AppDbContext _vContext = context;

        public async Task<(bool, (int, decimal))> CanPay(int menuId, int sizeId, CancellationToken cancellationToken = default)
        {
            var activePriceListId = await _vContext.PriceLists.AsNoTracking()
                                            .Where(x => x.Status == PriceListStatus.Active)
                                            .Select(x => (int?)x.Id)
                                            .FirstOrDefaultAsync(cancellationToken);

            if (activePriceListId is null) return (false, (0, 0m));
            var price = await (
                from m in _vContext.Menus.AsNoTracking()
                join g in _vContext.MenuGroups.AsNoTracking()
                                            on m.MenuGroupId
                                                equals g.Id
                join ms in _vContext.MenuSizes.AsNoTracking()
                                            on new { MenuID = m.Id, SizeID = sizeId }
                                                equals new { ms.MenuID, ms.SizeID }
                join pld in _vContext.PriceListDetails.AsNoTracking()
                                            on new { PriceListID = activePriceListId.Value, MenuID = m.Id, SizeID = sizeId }
                                                equals new { pld.PriceListID, pld.MenuID, pld.SizeID }
                where m.Id == menuId && m.Status == CommonStatus.Active && g.Status == CommonStatus.Active
                select (decimal?)pld.Price
            ).FirstOrDefaultAsync(cancellationToken);
            return price is null ? (false, (0, 0m)) : (true, (activePriceListId.Value, price.Value));
        }

        public async Task<Dictionary<(int MenuID, int SizeID), (bool CanPay, (int PriceListID, decimal Price) Data)>> CanPayBatch(
                                                                                                IReadOnlyCollection<(int MenuID, int SizeID)> items,
                                                                                                CancellationToken cancellationToken = default)
        {
            var result = new Dictionary<(int MenuID, int SizeID), (bool, (int, decimal))>(items.Count);
            foreach (var it in items.Distinct()) result[it] = (false, (0, 0m));

            if (result.Count == 0) return result;

            var activePriceListId = await _vContext.PriceLists.AsNoTracking()
                .Where(x => x.Status == PriceListStatus.Active)
                .Select(x => (int?)x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (activePriceListId is null) return result;

            var menuIds = result.Keys.Select(x => x.MenuID).Distinct().ToList();
            var sizeIds = result.Keys.Select(x => x.SizeID).Distinct().ToList();

            var rows = await (
                from m in _vContext.Menus.AsNoTracking()
                join g in _vContext.MenuGroups.AsNoTracking()
                     on m.MenuGroupId equals g.Id

                join ms in _vContext.MenuSizes.AsNoTracking()
                     on m.Id equals ms.MenuID

                join pld in _vContext.PriceListDetails.AsNoTracking()
                    on new { PriceListID = activePriceListId.Value, ms.MenuID, ms.SizeID }
                    equals new { pld.PriceListID, pld.MenuID, pld.SizeID }

                where menuIds.Contains(m.Id)
                      && sizeIds.Contains(ms.SizeID)
                      && m.Status == CommonStatus.Active
                      && g.Status == CommonStatus.Active

                select new
                {
                    ms.MenuID,
                    ms.SizeID,
                    pld.Price
                }
            ).ToListAsync(cancellationToken);

            foreach (var r in rows)
            {
                var key = (r.MenuID, r.SizeID);
                if (result.ContainsKey(key))
                    result[key] = (true, (activePriceListId.Value, r.Price));
            }

            return result;
        }

        public async Task<IReadOnlyList<RecipeConstractDto>> GetMenuRecipesAsync(IReadOnlyList<OrderItemsConstractDto> items, CancellationToken cancellationToken)
        {
            if (!items.Any()) return [];

            var groupedItems = items
                .GroupBy(x => new { x.MenuId, x.SizeId })
                .Select(g => new
                {
                    g.Key.MenuId,
                    g.Key.SizeId,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .ToList();

            var menuIds = groupedItems
                .Select(x => x.MenuId)
                .Distinct()
                .ToList();

            var recipes = await (from mam in _vContext.MenuMaterialRecipes.AsNoTracking()
                                 join m in _vContext.Materials.AsNoTracking()
                                     on mam.MaterialID equals m.Id
                                 where menuIds.Contains(mam.MenuID)
                                 select new
                                 {
                                     mam.MenuID,
                                     mam.SizeID,
                                     mam.MaterialID,
                                     mam.Quantity,
                                     m.Name
                                 })
            .ToListAsync(cancellationToken);

            var result = from mam in recipes
                         join oi in groupedItems
                             on new { mam.MenuID, mam.SizeID }
                             equals new { MenuID = oi.MenuId, SizeID = oi.SizeId }
                         group new { mam, oi }
                         by new { mam.MaterialID, mam.Name } into g
                         select new RecipeConstractDto
                         {
                             Id = g.Key.MaterialID,
                             Name = g.Key.Name,
                             Quantity = g.Sum(x => x.mam.Quantity * x.oi.Quantity)
                         };

            return result.ToList();
        }


        public async Task<IReadOnlyDictionary<int, MenuDto>> GetMenusAsync(IEnumerable<int> menuIds, CancellationToken cancellationToken = default)
        {
            return await (
                    from m in _vContext.Menus.AsNoTracking()
                    join g in _vContext.MenuGroups.AsNoTracking()
                        on m.MenuGroupId equals g.Id
                    join u in _vContext.Units.AsNoTracking()
                        on m.UnitId equals u.Id
                    where menuIds.Contains(m.Id)
                    select new MenuDto
                    {
                        MenuCode = m.Code,
                        MenuGroupId = m.MenuGroupId,
                        MenuGroupName = g.Name,
                        MenuId = m.Id,
                        MenuName = m.Name,
                        Status = new StatusDto
                        {
                            Id = (int)m.Status,
                            Name = m.Status.GetDescription()
                        },
                        UnitId = u.Id,
                        UnitName = u.Name,
                        Note = m.Note
                    }
                ).ToDictionaryAsync(x => x.MenuId, cancellationToken);
        }

        public async Task<IReadOnlyDictionary<int, SizeDto>> GetMenuSizesAsync(IEnumerable<int> sizeIds, CancellationToken cancellationToken = default)
        {
            return await _vContext.Sizes.AsNoTracking()
                                  .Where(s => sizeIds.Contains(s.Id))
                                  .Select(s => new SizeDto
                                  {
                                      SizeId = s.Id,
                                      SizeName = s.Name,
                                      RankIndex = s.RankIndex
                                  })
                                  .ToDictionaryAsync(x => x.SizeId, cancellationToken);
        }

        public async Task<TableDto?> GetTableAsync(int tableId, CancellationToken cancellationToken = default)
        {
            return await _vContext.Tables.AsNoTracking()
                                  .Where(t => t.Id == tableId)
                                  .Select(t => new TableDto
                                  {
                                      Id = t.Id,
                                      Code = t.Code,
                                      Name = t.Name,
                                      Position = t.Position,
                                      NumberOfSeats = t.NumberOfSeats,
                                      Status = new StatusDto()
                                      {
                                          Id = (int)t.Status,
                                          Name = t.Status.GetDescription()
                                      },
                                      Note = t.Note,
                                      UsingImg = $"data:image/png;base64,{Convert.ToBase64String(t.UsingPicture)}",
                                      EmptyImg = $"data:image/png;base64,{Convert.ToBase64String(t.EmptyPicture)}"
                                  })
                                  .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Dictionary<int, TableDto>> GetTableAsync(IReadOnlyCollection<int>? tableIds, CancellationToken cancellationToken = default)
        {
            if (tableIds == null || tableIds.Count == 0) return new Dictionary<int, TableDto>();

            var ids = tableIds.Count > 1 ? tableIds.Distinct() : tableIds;

            return await _vContext.Tables
                .AsNoTracking()
                .Where(t => ids.Contains(t.Id))
                .Select(t => new TableDto
                {
                    Id = t.Id,
                    Code = t.Code,
                    Name = t.Name,
                    Position = t.Position,
                    NumberOfSeats = t.NumberOfSeats,
                    Status = new StatusDto()
                    {
                        Id = (int)t.Status,
                        Name = t.Status.GetDescription()
                    },
                    Note = t.Note,

                    UsingImg = $"data:image/png;base64,{Convert.ToBase64String(t.UsingPicture)}",

                    EmptyImg = "data:image/png;base64,{Convert.ToBase64String(t.EmptyPicture)}"
                })
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }

        public async Task<bool> TableCanUse(int tableId, CancellationToken cancellationToken = default)
        {
            var row = await _vContext.Tables
                            .AsNoTracking()
                            .Where(t => t.Id == tableId && t.Status == TableStatus.InUsing)
                            .FirstOrDefaultAsync(cancellationToken);
            return row != null;
        }
    }
}