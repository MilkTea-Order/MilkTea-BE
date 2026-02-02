using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Catalog.Services;
using MilkTea.Application.Models.Catalog;
using MilkTea.Infrastructure.Persistence;
using Shared.Extensions;

namespace MilkTea.Infrastructure.Catalog.Services
{
    public class CatalogService(AppDbContext context) : ICatalogService
    {
        private readonly AppDbContext _vContext = context;
        public async Task<IReadOnlyDictionary<int, MenuItemDto>> GetMenusAsync(IEnumerable<int> menuIds, CancellationToken cancellationToken = default)
        {
            return await (
                    from m in _vContext.Menus.AsNoTracking()
                    join g in _vContext.MenuGroups.AsNoTracking()
                        on m.MenuGroupID equals g.Id
                    join u in _vContext.Units.AsNoTracking()
                        on m.UnitID equals u.Id
                    where menuIds.Contains(m.Id)
                    select new MenuItemDto
                    {
                        MenuCode = m.Code,
                        MenuGroupId = m.MenuGroupID,
                        MenuGroupName = g.Name,
                        MenuId = m.Id,
                        MenuName = m.Name,
                        StatusId = (int)m.Status,
                        StatusName = m.Status.GetDescription(),
                        UnitName = u.Name,
                        Note = m.Note
                    }
                ).ToDictionaryAsync(x => x.MenuId, cancellationToken);
        }

        public async Task<IReadOnlyDictionary<int, MenuSizeDto>> GetMenuSizesAsync(IEnumerable<int> sizeIds, CancellationToken cancellationToken = default)
        {
            return await _vContext.Sizes.AsNoTracking()
                                  .Where(s => sizeIds.Contains(s.Id))
                                  .Select(s => new MenuSizeDto
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
                                      StatusId = (int?)t.Status,
                                      StatusName = t.Status.GetDescription(),
                                      Note = t.Note,
                                      UsingImg = t.UsingPicture != null
                                                   ? $"data:image/png;base64,{Convert.ToBase64String(t.UsingPicture)}"
                                                   : null,
                                      EmptyImg = t.EmptyPicture != null
                                                   ? $"data:image/png;base64,{Convert.ToBase64String(t.EmptyPicture)}"
                                                   : null
                                  })
                                  .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
