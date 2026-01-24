using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Domain.Respositories.Orders
{
    public interface IMenuRepository
    {
        // Read methods for menu groups
        Task<List<MenuGroup>> GetMenuGroupsByStatusAsync(int? groupStatusID, int? sellingStatusID);
        Task<List<MenuGroup>> GetMenuGroupsAvailableAsync();
        Task<MenuGroup?> GetMenuGroupByID(int groupID);
        Task<MenuGroup?> GetMenuGroupByIDAndAvaliableAsync(int groupID);

        // Read methods for menu items
        Task<List<Menu>> GetMenusOfGroupByStatusAsync(int groupID, int? itemStatusID);
        Task<List<Menu>> GetMenusAvailableOfGroupAsync(int groupID);
        Task<Menu?> GetMenuByIDAsync(int menuId);
        Task<Menu?> GetMenuByIDAndAvaliableAsync(int menuID);

        // Read methods for menu sizes
        Task<List<MenuSize>> GetMenuSizesByMenuAsync(int menuID);
        Task<List<MenuSize>> GetMenuSizesAvailableByMenuAsync(int menuID);
    }
}
