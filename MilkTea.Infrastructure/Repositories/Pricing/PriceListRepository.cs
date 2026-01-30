using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Pricing.Entities;
using MilkTea.Domain.Pricing.Enums;
using MilkTea.Domain.Pricing.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Pricing;

/// <summary>
/// Repository implementation for price list operations.
/// </summary>
public class PriceListRepository(AppDbContext context) : IPriceListRepository
{
    private readonly AppDbContext _vContext = context;

    /// <inheritdoc/>
    public async Task<PriceList?> GetByIdAsync(int id)
    {
        return await _vContext.PriceLists
            .AsNoTracking()
            .Include(pl => pl.Currency)
            .Include(pl => pl.Details)
            .FirstOrDefaultAsync(pl => pl.Id == id);
    }

    /// <inheritdoc/>
    public async Task<PriceList?> GetActivePriceListAsync()
    {
        var now = DateTime.UtcNow;
        return await _vContext.PriceLists
            .AsNoTracking()
            .Include(pl => pl.Currency)
            .Include(pl => pl.Details)
            .Where(pl => pl.Status == PriceListStatus.Active
                && pl.StartDate <= now
                && pl.StopDate >= now)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<decimal?> GetPriceAsync(int priceListId, int menuId, int sizeId)
    {
        var detail = await _vContext.PriceListDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(pld => pld.PriceListID == priceListId
                && pld.MenuID == menuId
                && pld.SizeID == sizeId);

        return detail?.Price;
    }

    /// <inheritdoc/>
    public async Task<List<PriceListDetail>> GetDetailsByPriceListIdAsync(int priceListId)
    {
        return await _vContext.PriceListDetails
            .AsNoTracking()
            .Where(pld => pld.PriceListID == priceListId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Dictionary<int, decimal>> GetPricesForMenuAsync(int priceListId, int menuId, CancellationToken cancellationToken)
    {
        var details = await _vContext.PriceListDetails
            .AsNoTracking()
            .Where(pld => pld.PriceListID == priceListId && pld.MenuID == menuId)
            .ToListAsync(cancellationToken);

        return details.ToDictionary(d => d.SizeID, d => d.Price);
    }
}
