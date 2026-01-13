using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Domain.Respositories.Orders
{
    public interface IWarehouseRepository
    {
        Task<List<MenuAndMaterial>> GetRecipeAsync(int menuId, int sizeId);
        Task<Dictionary<int, decimal>> GetMaterialStockAsync(List<int> materialIds);
        Task DeductMaterialStockAsync(int materialId, decimal quantity);
        Task<WarehouseRollback> CreateStockHistoryAsync(WarehouseRollback stockHistory);
        Task<List<Warehouse>> GetWarehousesByMaterialIdAsync(int materialId);
    }
}