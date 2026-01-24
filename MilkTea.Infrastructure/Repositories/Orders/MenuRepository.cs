using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Constants.Enums;
using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Infrastructure.Persistence;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Infrastructure.Repositories.Orders
{
    public sealed class MenuRepository(AppDbContext context) : IMenuRepository
    {
        private readonly AppDbContext _vContext = context;

        #region Menu Group
        public async Task<MenuGroup?> GetMenuGroupByID(int groupID)
        {
            return await _vContext.MenuGroup
                .AsNoTracking()
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.ID == groupID);
        }

        public async Task<List<MenuGroup>> GetMenuGroupsAvailableAsync()
        {
            return await _vContext.MenuGroup
                .AsNoTracking()
                .Include(x => x.Status)
                .Where(mg => mg.Status != null && mg.Status.Name == StatusName.NAME_ACTIVE)
                .ToListAsync();
        }

        public async Task<MenuGroup?> GetMenuGroupByIDAndAvaliableAsync(int groupID)
        {
            return await _vContext.MenuGroup
                .AsNoTracking()
                .Include(x => x.Status)
                .Where(mg => mg.ID == groupID && mg.Status != null && mg.Status.Name == StatusName.NAME_ACTIVE)
                .FirstOrDefaultAsync();
        }

        public async Task<List<MenuGroup>> GetMenuGroupsByStatusAsync(int? groupStatusID, int? sellingStatusID)
        {
            var query = _vContext.MenuGroup
                .AsNoTracking()
                .Include(x => x.Status)
                .AsQueryable();

            if (groupStatusID.HasValue)
            {
                query = query.Where(x => x.StatusID == groupStatusID.Value);
            }

            return await query.ToListAsync();
        }
        #endregion

        #region Menu
        public async Task<Menu?> GetMenuByIDAsync(int menuId)
        {
            return await _vContext.Menu
                .AsNoTracking()
                .Include(x => x.MenuGroup)
                .Include(x => x.Status)
                .Include(x => x.Unit)
                .FirstOrDefaultAsync(m => m.ID == menuId);
        }

        public Task<Menu?> GetMenuByIDAndAvaliableAsync(int menuID)
        {
            return _vContext.Menu
                .AsNoTracking()
                .Include(x => x.MenuGroup)
                .Include(x => x.Status)
                .Include(x => x.Unit)
                .Where(m => m.ID == menuID && m.Status != null && m.Status.Name == StatusName.NAME_ACTIVE)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Menu>> GetMenusOfGroupByStatusAsync(int groupID, int? itemStatusID)
        {
            var query = _vContext.Menu
                .AsNoTracking()
                .Include(x => x.MenuGroup)
                .Include(x => x.Status)
                .Where(m => m.MenuGroupID == groupID);

            if (itemStatusID.HasValue)
            {
                query = query.Where(x => x.StatusID == itemStatusID.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<List<Menu>> GetMenusAvailableOfGroupAsync(int groupID)
        {
            return await _vContext.Menu
                .AsNoTracking()
                .Include(x => x.MenuGroup)
                .Include(x => x.Status)
                .Where(m => m.MenuGroupID == groupID && m.Status != null && m.Status.Name == StatusName.NAME_ACTIVE)
                .ToListAsync();
        }
        #endregion

        #region Menu Size
        public async Task<List<MenuSize>> GetMenuSizesByMenuAsync(int menuID)
        {
            return await _vContext.MenuSize
                .AsNoTracking()
                .Include(x => x.Size)
                .Where(ms => ms.MenuID == menuID)
                .OrderBy(ms => ms.Size != null ? ms.Size.RankIndex : 0)
                .ToListAsync();
        }

        public async Task<List<MenuSize>> GetMenuSizesAvailableByMenuAsync(int menuID)
        {
            var now = DateTime.UtcNow;
            var query = from ms in _vContext.MenuSize
                        join s in _vContext.Size on ms.SizeID equals s.ID
                        join pld in _vContext.PriceListDetail
                            on new { ms.MenuID, ms.SizeID } equals new { pld.MenuID, pld.SizeID }
                        join pl in _vContext.PriceList on pld.PriceListID equals pl.ID
                        where ms.MenuID == menuID
                              && pl.StatusOfPriceListID == (int)PriceListStatus.Active
                              && pl.StartDate <= now
                              && pl.StopDate >= now
                        orderby s.RankIndex
                        select ms;

            return await query
                .AsNoTracking()
                .Include(ms => ms.Size)
                .Distinct()
                .ToListAsync();
        }
        #endregion
    }
}

