using MilkTea.Domain.Catalog.Material.Entities;
using MilkTea.Domain.Catalog.Menu.Entities;

namespace MilkTea.Domain.Catalog.Material.Repositories;

/// <summary>
/// Repository interface for material operations.
/// </summary>
public interface IMaterialRepository
{
    /// <summary>
    /// Gets a material by ID.
    /// </summary>
    Task<MaterialEntity?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all materials.
    /// </summary>
    Task<List<MaterialEntity>> GetAllAsync();

    /// <summary>
    /// Gets materials by group ID.
    /// </summary>
    Task<List<MaterialEntity>> GetByGroupIdAsync(int groupId);

    /// <summary>
    /// Gets recipe materials for a menu item.
    /// </summary>
    Task<List<MenuMaterialRecipeEntity>> GetRecipeByMenuIdAsync(int menuId);
}
