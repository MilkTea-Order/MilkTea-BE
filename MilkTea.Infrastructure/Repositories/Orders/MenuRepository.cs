using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Constants.Enums;
using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Infrastructure.Persistence;
using MilkTea.Shared.Domain.Constants;
using MilkTea.Shared.Extensions;

namespace MilkTea.Infrastructure.Repositories.Orders
{
    public class MenuRepository(AppDbContext context) : IMenuRepository
    {
        private readonly AppDbContext _vContext = context;

        // Get menu groups
        public async Task<List<Dictionary<string, object?>>> GetMenuGroupByStatusAsync(int? groupStatusID, int? sellingStatusID)
        {
            var query =
                from mg in _vContext.MenuGroup.AsNoTracking()
                join st in _vContext.Status.AsNoTracking()
                    on mg.StatusID equals st.ID into sts
                from st in sts.DefaultIfEmpty()

                join m in _vContext.Menu.AsNoTracking()
                    on mg.ID equals m.MenuGroupID into menus

                select new
                {
                    MenuGroupID = mg.ID,
                    MenuGroupName = mg.Name,
                    StatusID = mg.StatusID,
                    StatusName = st != null ? st.Name : "Không rõ",
                    Quantity = sellingStatusID.HasValue
                        ? menus.Count(x => x.StatusID == sellingStatusID.Value)
                        : menus.Count()
                };

            if (groupStatusID.HasValue)
            {
                query = query.Where(x => x.StatusID == groupStatusID.Value);
            }

            return (await query.ToListAsync()).ToDictList();
        }
        public async Task<List<Dictionary<string, object?>>> GetMenuGroupAvaliableAsync()
        {
            var row = await (
                from mg in _vContext.MenuGroup.AsNoTracking()

                join stGroup in _vContext.Status.AsNoTracking()
                    on mg.StatusID equals stGroup.ID

                join m in _vContext.Menu.AsNoTracking()
                    on mg.ID equals m.MenuGroupID into menus

                where stGroup.Name == StatusName.NAME_ACTIVE

                select new
                {
                    MenuGroupID = mg.ID,
                    MenuGroupName = mg.Name,
                    StatusID = mg.StatusID,
                    StatusName = stGroup.Name,

                    Quantity =
                        (from m in menus
                         join stMenu in _vContext.Status.AsNoTracking()
                             on m.StatusID equals stMenu.ID
                         where stMenu.Name == StatusName.NAME_ACTIVE
                         select m
                        ).Count()
                }
            ).ToListAsync();

            return row.ToDictList();
        }

        public async Task<MenuGroup?> GetMenuGroupByID(int groupID)
        {
            return await _vContext.MenuGroup.AsNoTracking().FirstOrDefaultAsync(x => x.ID == groupID);
        }
        public async Task<MenuGroup?> GetMenuGroupByIDAndAvaliableAsync(int groupID)
        {
            return await (
                from mg in _vContext.MenuGroup.AsNoTracking()
                join s in _vContext.Status.AsNoTracking()
                    on mg.StatusID equals s.ID
                where mg.ID == groupID
                      && s.Name == StatusName.NAME_ACTIVE
                select mg
            ).FirstOrDefaultAsync();
        }


        // Get menu items
        public async Task<List<Dictionary<string, object?>>> GetMenuItemsOfGroupByStatusAsync(int groupID, int? itemStatusID)
        {
            var query =
                from m in _vContext.Menu.AsNoTracking()
                join mg in _vContext.MenuGroup.AsNoTracking()
                    on m.MenuGroupID equals mg.ID into mgs
                from mg in mgs.DefaultIfEmpty()
                join st in _vContext.Status.AsNoTracking()
                    on m.StatusID equals st.ID into sts
                from st in sts.DefaultIfEmpty()
                where m.MenuGroupID == groupID
                select new
                {
                    MenuID = m.ID,
                    MenuCode = m.Code,
                    MenuName = m.Name,

                    MenuGroupID = m.MenuGroupID,
                    MenuGroupName = mg != null ? mg.Name : null,

                    StatusID = m.StatusID,
                    StatusName = st != null ? st.Name : "Không rõ"
                };
            if (itemStatusID.HasValue)
            {
                query = query.Where(x => x.StatusID == itemStatusID.Value);
            }

            return (await query.ToListAsync()).ToDictList();
        }
        public async Task<List<Dictionary<string, object?>>> GetMenuItemsAvaliableOfGroupByStatusAsync(int groupID)
        {
            var query =
                from m in _vContext.Menu.AsNoTracking()
                join mg in _vContext.MenuGroup.AsNoTracking()
                    on m.MenuGroupID equals mg.ID into mgs
                from mg in mgs.DefaultIfEmpty()
                join st in _vContext.Status.AsNoTracking()
                    on m.StatusID equals st.ID into sts
                from st in sts.DefaultIfEmpty()
                where m.MenuGroupID == groupID
                    && st.Name == StatusName.NAME_ACTIVE
                select new
                {
                    MenuID = m.ID,
                    MenuCode = m.Code,
                    MenuName = m.Name,
                    MenuGroupID = m.MenuGroupID,
                    MenuGroupName = mg != null ? mg.Name : null,
                    StatusID = m.StatusID,
                    StatusName = st != null ? st.Name : "Không rõ"
                };

            return (await query.ToListAsync()).ToDictList();
        }

        // Get menu 
        public async Task<Menu?> GetMenuByIDAsync(int menuId)
        {
            return await _vContext.Menu.AsNoTracking().FirstOrDefaultAsync(m => m.ID == menuId);
        }

        public Task<Menu?> GetMenuByIDAndAvaliableAsync(int menuID)
        {
            var query =
                from m in _vContext.Menu.AsNoTracking()
                join s in _vContext.Status.AsNoTracking()
                    on m.StatusID equals s.ID
                where m.ID == menuID
                      && s.Name == StatusName.NAME_ACTIVE
                select m;
            return query.FirstOrDefaultAsync();
        }

        // Get menu sizes
        public async Task<List<Dictionary<string, object?>>> GetMenuSizeByMenuAsync(int menuID)
        {
            var query =
               from ms in _vContext.MenuSize.AsNoTracking()

               join s in _vContext.Size.AsNoTracking()
                   on ms.SizeID equals s.ID into ss
               from s in ss.DefaultIfEmpty()

               where ms.MenuID == menuID

               select new
               {
                   SizeID = ms.SizeID,
                   SizeName = s != null ? s.Name : null,
                   RankIndex = s.RankIndex
               };

            return (await query.ToListAsync()).ToDictList();
        }

        public async Task<List<Dictionary<string, object?>>> GetMenuSizeWithPriceByMenuAsync(int menuID)
        {
            var now = DateTime.UtcNow;

            var query =
                from ms in _vContext.MenuSize.AsNoTracking()

                join s in _vContext.Size.AsNoTracking()
                    on ms.SizeID equals s.ID

                join pld in _vContext.PriceListDetail.AsNoTracking()
                    on new { ms.MenuID, ms.SizeID }
                    equals new { pld.MenuID, pld.SizeID }

                join pl in _vContext.PriceList.AsNoTracking()
                    on pld.PriceListID equals pl.ID

                join cu in _vContext.Currency.AsNoTracking()
                    on pl.CurrencyID equals cu.ID

                where ms.MenuID == menuID
                      && pl.StatusOfPriceListID == (int)PriceListStatus.Active
                      && pl.StartDate <= now
                      && pl.StopDate >= now

                select new
                {
                    SizeID = s.ID,
                    SizeName = s.Name,
                    RankIndex = s.RankIndex,
                    Price = pld.Price,
                    CurrencyName = cu.Name,
                    CurrencyCode = cu.Code
                };

            return (await query
                .OrderBy(x => x.RankIndex)
                .ToListAsync())
                .ToDictList();
        }





    }
}
