using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Catalog.Entities;
using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Domain.Pricing.Enums;
using MilkTea.Domain.SharedKernel.Enums;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Catalog;

/// <summary>
/// Repository implementation for menu-related data operations.
/// </summary>
public sealed class MenuRepository(AppDbContext context) : IMenuRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc/>
    public async Task<List<MenuGroup>> GetAllMenuGroupsAsync()
    {
        return await _context.MenuGroups
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<MenuGroup>> GetActiveMenuGroupsAsync()
    {
        return await _context.MenuGroups
            .AsNoTracking()
            .Where(mg => mg.Status == CommonStatus.Active)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<Menu>> GetMenuItemsByGroupIdAsync(int groupId)
    {
        return await _context.Menus
            .AsNoTracking()
            .Include(m => m.MenuGroup)
            .Where(m => m.MenuGroupID == groupId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<Menu>> GetActiveMenuItemsByGroupIdAsync(int groupId)
    {
        return await _context.Menus
            .AsNoTracking()
            .Include(m => m.MenuGroup)
            .Where(m => m.MenuGroupID == groupId && m.Status == MenuStatus.Active)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<MenuSize>> GetMenuSizesByMenuIdAsync(int menuId)
    {
        return await _context.MenuSizes
            .AsNoTracking()
            .Include(ms => ms.Size)
            .Where(ms => ms.MenuID == menuId)
            .OrderBy(ms => ms.Size != null ? ms.Size.RankIndex : 0)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Menu?> GetMenuByIdAsync(int menuId)
    {
        return await _context.Menus
            .AsNoTracking()
            .Include(m => m.MenuGroup)
            .FirstOrDefaultAsync(m => m.Id == menuId);
    }

    /// <inheritdoc/>
    public async Task<List<Size>> GetAllSizesAsync()
    {
        return await _context.Sizes
            .AsNoTracking()
            .OrderBy(s => s.RankIndex)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<MenuGroup>> GetMenuGroupsByStatusAsync(int? statusId, int? itemStatusId)
    {
        var query = _context.MenuGroups.AsNoTracking();

        if (statusId.HasValue)
        {
            var status = (CommonStatus)statusId.Value;
            query = query.Where(mg => mg.Status == status);
        }

        if (itemStatusId.HasValue)
        {
            var menuStatus = (MenuStatus)itemStatusId.Value;
            query = query.Where(mg => mg.Menus.Any(m => m.Status == menuStatus));
        }

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<MenuGroup>> GetMenuGroupsAvailableAsync()
    {
        return await _context.MenuGroups
            .AsNoTracking()
            .Where(mg => mg.Status == CommonStatus.Active 
                && mg.Menus.Any(m => m.Status == MenuStatus.Active))
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Dictionary<int, int>> GetMenuCountsByGroupIdsAsync(List<int> groupIds)
    {
        return await _context.Menus
            .AsNoTracking()
            .Where(m => groupIds.Contains(m.MenuGroupID))
            .GroupBy(m => m.MenuGroupID)
            .Select(g => new { GroupId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.GroupId, x => x.Count);
    }

    /// <inheritdoc/>
    public async Task<List<Menu>> GetMenusOfGroupByStatusAsync(int groupId, int? menuStatusId)
    {
        var query = _context.Menus
            .AsNoTracking()
            .Include(m => m.MenuGroup)
            .Where(m => m.MenuGroupID == groupId);

        if (menuStatusId.HasValue)
        {
            var menuStatus = (MenuStatus)menuStatusId.Value;
            query = query.Where(m => m.Status == menuStatus);
        }

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<MenuSize>> GetMenuSizesAvailableByMenuAsync(int menuId)
    {
        return await _context.MenuSizes
            .AsNoTracking()
            .Include(ms => ms.Size)
            .Where(ms => ms.MenuID == menuId 
                && ms.Menu != null 
                && ms.Menu.Status == MenuStatus.Active)
            .OrderBy(ms => ms.Size != null ? ms.Size.RankIndex : 0)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Menu?> GetMenuByIDAndAvaliableAsync(int menuId)
    {
        return await _context.Menus
            .AsNoTracking()
            .Include(m => m.MenuGroup)
            .Where(m => m.Id == menuId 
                && m.Status == MenuStatus.Active
                && m.MenuGroup != null
                && m.MenuGroup.Status == CommonStatus.Active)
            .FirstOrDefaultAsync();
    }
}
