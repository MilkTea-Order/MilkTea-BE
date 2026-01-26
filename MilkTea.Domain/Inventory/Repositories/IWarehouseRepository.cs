using MilkTea.Domain.Inventory.Entities;

namespace MilkTea.Domain.Inventory.Repositories;

/// <summary>
/// Repository interface for warehouse operations.
/// </summary>
public interface IWarehouseRepository
{
    /// <summary>
    /// Gets warehouse entry by ID.
    /// </summary>
    Task<Warehouse?> GetByIdAsync(int id);

    /// <summary>
    /// Gets warehouse entry by material ID.
    /// </summary>
    Task<Warehouse?> GetByMaterialIdAsync(int materialId);

    /// <summary>
    /// Gets all active warehouse entries.
    /// </summary>
    Task<List<Warehouse>> GetAllActiveAsync();

    /// <summary>
    /// Updates a warehouse entry.
    /// </summary>
    Task<bool> UpdateAsync(Warehouse warehouse);

    /// <summary>
    /// Adds a rollback record.
    /// </summary>
    Task AddRollbackAsync(WarehouseRollback rollback);

    /// <summary>
    /// Checks if materials are in stock for a menu item.
    /// </summary>
    Task<bool> CheckStockForMenuItemAsync(int menuId, int quantity);

    /// <summary>
    /// Gets recipe (materials) for a menu item and size.
    /// </summary>
    Task<List<MenuMaterialRecipe>?> GetRecipeAsync(int menuId, int sizeId);

    /// <summary>
    /// Gets material stock by material IDs.
    /// </summary>
    Task<Dictionary<int, decimal>> GetMaterialStockAsync(List<int> materialIds);

    /// <summary>
    /// Gets material names by material IDs.
    /// </summary>
    Task<Dictionary<int, string>> GetMaterialsAsync(List<int> materialIds);
}
