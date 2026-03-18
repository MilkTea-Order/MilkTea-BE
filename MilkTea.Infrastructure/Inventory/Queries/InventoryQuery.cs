using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Inventory.Abstractions;
using MilkTea.Application.Features.Inventory.Models.Dtos;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Inventory.Queries
{
    public class InventoryQuery(AppDbContext context) : IInventoryQuery
    {
        private readonly AppDbContext _vContext = context;
        public async Task<List<InventoryStockDto>> GetInventoryReportAsync()
        {
            var query = _vContext.Warehouses
                                    .AsNoTracking()
                                    .Where(w => w.Status == Domain.Inventory.Enums.InventoryStatus.InStock && w.QuantityCurrent > 0)
                                    .Join(
                                        _vContext.ImportFromSuppliers.AsNoTracking(),
                                        w => w.ImportFromSuppliersID,
                                        i => i.Id,
                                        (w, i) => new
                                        {
                                            w.MaterialsID,
                                            w.QuantityCurrent,
                                            w.PriceImport,
                                            i.ImportedDate,
                                            w.Id
                                        }
                                    )
                                    .GroupBy(x => x.MaterialsID)
                                    .Select(g => new InventoryStockDto
                                    {
                                        MaterialId = g.Key,
                                        TotalQuantity = g.Sum(x => x.QuantityCurrent),
                                        LatestPriceImport = g.OrderByDescending(x => x.ImportedDate)
                                                            .ThenByDescending(x => x.Id)
                                                            .Select(x => x.PriceImport)
                                                            .FirstOrDefault()
                                    });
            return await query.ToListAsync();
        }
    }
}

