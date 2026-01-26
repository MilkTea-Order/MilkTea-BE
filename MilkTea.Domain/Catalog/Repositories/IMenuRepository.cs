using MilkTea.Domain.Catalog.Entities;

namespace MilkTea.Domain.Catalog.Repositories;

/// <summary>
/// Repository interface for menu-related data operations.
/// </summary>
public interface IMenuRepository
{
    /// <summary>
    /// Gets all menu groups.
    /// </summary>
    Task<List<MenuGroup>> GetAllMenuGroupsAsync();

    /// <summary>
    /// Gets all active menu groups.
    /// </summary>
    Task<List<MenuGroup>> GetActiveMenuGroupsAsync();

    /// <summary>
    /// Gets menu items by group ID.
    /// </summary>
    Task<List<Menu>> GetMenuItemsByGroupIdAsync(int groupId);

    /// <summary>
    /// Gets active menu items by group ID.
    /// </summary>
    Task<List<Menu>> GetActiveMenuItemsByGroupIdAsync(int groupId);

    /// <summary>
    /// Gets menu sizes for a menu item.
    /// </summary>
    Task<List<MenuSize>> GetMenuSizesByMenuIdAsync(int menuId);

    /// <summary>
    /// Gets a menu item by ID.
    /// </summary>
    Task<Menu?> GetMenuByIdAsync(int menuId);

    /// <summary>
    /// Gets all sizes.
    /// </summary>
    Task<List<Size>> GetAllSizesAsync();

    /// <summary>
    /// Gets menu groups by status.
    /// </summary>
    Task<List<MenuGroup>> GetMenuGroupsByStatusAsync(int? statusId, int? itemStatusId);

    /// <summary>
    /// Gets available menu groups.
    /// </summary>
    Task<List<MenuGroup>> GetMenuGroupsAvailableAsync();

    /// <summary>
    /// Gets menu counts by group IDs.
    /// </summary>
    Task<Dictionary<int, int>> GetMenuCountsByGroupIdsAsync(List<int> groupIds);

    /// <summary>
    /// Gets menus of group by status.
    /// </summary>
    Task<List<Menu>> GetMenusOfGroupByStatusAsync(int groupId, int? menuStatusId);

    /// <summary>
    /// Gets menu sizes available by menu.
    /// </summary>
    Task<List<MenuSize>> GetMenuSizesAvailableByMenuAsync(int menuId);

    /// <summary>
    /// Gets menu by ID and available.
    /// </summary>
    Task<Menu?> GetMenuByIDAndAvaliableAsync(int menuId);
}
