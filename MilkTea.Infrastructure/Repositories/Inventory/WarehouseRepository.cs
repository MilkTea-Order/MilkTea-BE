using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Inventory.Entities;
using MilkTea.Domain.Inventory.Repositories;
using MilkTea.Domain.SharedKernel.Enums;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Inventory;

/// <summary>
/// Repository implementation for warehouse operations.
/// </summary>
public class WarehouseRepository(AppDbContext context) : IWarehouseRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc/>
    public async Task<Warehouse?> GetByIdAsync(int id)
    {
        return await _context.Warehouses
            .AsNoTracking()
            .Include(w => w.Material)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    /// <inheritdoc/>
    public async Task<Warehouse?> GetByMaterialIdAsync(int materialId)
    {
        return await _context.Warehouses
            .AsNoTracking()
            .Include(w => w.Material)
            .FirstOrDefaultAsync(w => w.MaterialsID == materialId && w.Status == CommonStatus.Active);
    }

    /// <inheritdoc/>
    public async Task<List<Warehouse>> GetAllActiveAsync()
    {
        return await _context.Warehouses
            .AsNoTracking()
            .Include(w => w.Material)
            .Where(w => w.Status == CommonStatus.Active)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(Warehouse warehouse)
    {
        _context.Warehouses.Update(warehouse);
        return await _context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc/>
    public async Task AddRollbackAsync(WarehouseRollback rollback)
    {
        await _context.WarehouseRollbacks.AddAsync(rollback);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> CheckStockForMenuItemAsync(int menuId, int quantity)
    {
        // Get recipe for menu
        var recipe = await _context.MenuMaterialRecipes
            .AsNoTracking()
            .Where(mmr => mmr.MenuID == menuId)
            .ToListAsync();

        if (recipe.Count == 0)
            return false;

        // Check stock for each material
        foreach (var item in recipe)
        {
            var warehouse = await GetByMaterialIdAsync(item.MaterialID);
            if (warehouse == null || warehouse.QuantityCurrent < (item.Quantity * quantity))
                return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public async Task<List<MenuMaterialRecipe>?> GetRecipeAsync(int menuId, int sizeId)
    {
        // Note: SizeId might not be directly in MenuMaterialRecipe
        // Assuming recipe is per menu, not per menu+size combination
        return await _context.MenuMaterialRecipes
            .AsNoTracking()
            .Include(mmr => mmr.Material)
            //.Include(mmr => mmr.Unit)
            .Where(mmr => mmr.MenuID == menuId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Dictionary<int, decimal>> GetMaterialStockAsync(List<int> materialIds)
    {
        var warehouses = await _context.Warehouses
            .AsNoTracking()
            .Where(w => materialIds.Contains(w.MaterialsID) && w.Status == CommonStatus.Active)
            .ToListAsync();

        return warehouses.ToDictionary(w => w.MaterialsID, w => w.QuantityCurrent);
    }

    /// <inheritdoc/>
    public async Task<Dictionary<int, string>> GetMaterialsAsync(List<int> materialIds)
    {
        var materials = await _context.Materials
            .AsNoTracking()
            .Where(m => materialIds.Contains(m.Id))
            .ToListAsync();

        return materials.ToDictionary(m => m.Id, m => m.Name ?? $"MaterialID:{m.Id}");
    }
}
