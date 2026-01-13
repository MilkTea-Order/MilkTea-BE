using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Orders
{
    public class WarehouseRepository(AppDbContext context) : IWarehouseRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<MenuAndMaterial>> GetRecipeAsync(int menuId, int sizeId)
        {
            return await _context.MenuAndMaterials
                .AsNoTracking()
                .Where(mm => mm.MenuID == menuId && mm.SizeID == sizeId)
                .ToListAsync();
        }

        public async Task<Dictionary<int, decimal>> GetMaterialStockAsync(List<int> materialIds)
        {
            var stocks = await _context.Warehouse
                .AsNoTracking()
                .Where(w => materialIds.Contains(w.MaterialsID) && w.QuantityCurrent > 0)
                .GroupBy(w => w.MaterialsID)
                .Select(g => new
                {
                    MaterialId = g.Key,
                    TotalStock = g.Sum(w => w.QuantityCurrent)
                })
                .ToListAsync();

            return stocks.ToDictionary(s => s.MaterialId, s => s.TotalStock);
        }

        public async Task<List<Warehouse>> GetWarehousesByMaterialIdAsync(int materialId)
        {
            return await _context.Warehouse
                .Where(w => w.MaterialsID == materialId && w.QuantityCurrent > 0)
                .OrderBy(w => w.ID) // FIFO
                .ToListAsync();
        }

        public async Task DeductMaterialStockAsync(int warehouseId, decimal quantity)
        {
            var warehouse = await _context.Warehouse.FindAsync(warehouseId);
            if (warehouse != null)
            {
                warehouse.QuantityCurrent -= quantity;
            }
        }

        public async Task<WarehouseRollback> CreateStockHistoryAsync(WarehouseRollback stockHistory)
        {
            await _context.WarehouseRollback.AddAsync(stockHistory);
            return stockHistory;
        }
    }
}