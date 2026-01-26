using MilkTea.Domain.Inventory.Entities;

namespace MilkTea.Domain.Inventory.Repositories;

/// <summary>
/// Repository interface for material operations.
/// </summary>
public interface IMaterialRepository
{
    /// <summary>
    /// Gets a material by ID.
    /// </summary>
    Task<Material?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all materials.
    /// </summary>
    Task<List<Material>> GetAllAsync();

    /// <summary>
    /// Gets materials by group ID.
    /// </summary>
    Task<List<Material>> GetByGroupIdAsync(int groupId);

    /// <summary>
    /// Gets recipe materials for a menu item.
    /// </summary>
    Task<List<MenuMaterialRecipe>> GetRecipeByMenuIdAsync(int menuId);
}
