using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Inventory.Entities;
using MilkTea.Domain.Inventory.Enums;
using MilkTea.Domain.Inventory.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Inventory;

public class WarehouseRepository(AppDbContext context) : IWarehouseRepository
{
    private readonly AppDbContext _vContext = context;

    /// <inheritdoc/>
    public async Task<WarehouseEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _vContext.Warehouses.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<WarehouseEntity>> GetActiveByMaterialIdsAsync(IEnumerable<int> materialIds, CancellationToken cancellationToken)
    {
        return await _vContext.Warehouses.Where(x => materialIds.Contains(x.MaterialsID) && x.Status == InventoryStatus.InStock && x.QuantityCurrent > 0)
                                          .GroupBy(x => x.MaterialsID)
                                          .Select(g => g.OrderBy(x => x.Id).First())
                                          .ToListAsync(cancellationToken);
    }
}
