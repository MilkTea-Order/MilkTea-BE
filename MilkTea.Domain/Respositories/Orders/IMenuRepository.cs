using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Domain.Respositories.Orders
{
    public interface IMenuRepository
    {
        Task<List<Dictionary<string, object?>>> GetMenuGroupByStatusAsync(int? statusID, int? sellingStatusID);
        Task<List<Dictionary<string, object?>>> GetMenuGroupAvaliableAsync();
        Task<List<Dictionary<string, object?>>> GetMenuItemsOfGroupByStatusAsync(int groupID, int? itemStatusID);
        Task<List<Dictionary<string, object?>>> GetMenuItemsAvaliableOfGroupByStatusAsync(int groupID);
        Task<List<Dictionary<string, object?>>> GetMenuSizeByMenuAsync(int menuID);
        Task<List<Dictionary<string, object?>>> GetMenuSizeWithPriceByMenuAsync(int menuID);
        Task<MenuGroup?> GetMenuGroupByID(int groupID);
        Task<MenuGroup?> GetMenuGroupByIDAndAvaliableAsync(int groupID);
        Task<Menu?> GetMenuByIDAsync(int menuId);
        Task<Menu?> GetMenuByIDAndAvaliableAsync(int menuID);

    }
}
