using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Catalog.Entities.Price;
using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Catalog;

/// <summary>
/// Repository implementation for price list operations.
/// </summary>
public class PriceListRepository(AppDbContext context) : IPriceListRepository
{
    private readonly AppDbContext _vContext = context;


    /// <inheritdoc/>
    public async Task<PriceListEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _vContext.PriceLists
            .AsNoTracking()
            .Include(pl => pl.Currency)
            .Include(pl => pl.Details)
            .FirstOrDefaultAsync(pl => pl.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<PriceListEntity?> GetActiveWithCurrencyAsync(CancellationToken cancellationToken = default)
    {
        return await _vContext.PriceLists
            .AsNoTracking()
            .Include(pl => pl.Currency)
            .Where(pl => pl.Status == PriceListStatus.Active)
            .OrderByDescending(pl => pl.StartDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<PriceListEntity?> GetActiveWithRelationshipAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _vContext.PriceLists
            .AsNoTracking()
            .Include(pl => pl.Currency)
            .Include(pl => pl.Details)
            .Where(pl => pl.Status == PriceListStatus.Active)
            .FirstOrDefaultAsync();
    }


    public async Task<PriceListEntity?> GetActiveByMenuAndSizeWithRelationshipAsync(int menuId, int sizeId, CancellationToken cancellationToken = default)
    {
        return await _vContext.PriceLists
            .AsNoTracking()
            .AsSplitQuery()
            .Include(pl => pl.Currency)
            .Include(pl => pl.Details.Where(d => d.MenuID == menuId && d.SizeID == sizeId))
            .Where(pl => pl.Status == PriceListStatus.Active)
            .OrderByDescending(pl => pl.StartDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<PriceListEntity?> GetActiveByMenuWithRelationshipAsync(int menuId, CancellationToken cancellationToken)
    {
        return await _vContext.PriceLists
            .AsNoTracking()
            .AsSplitQuery()
            .Include(pl => pl.Currency)
            .Include(pl => pl.Details.Where(d => d.MenuID == menuId))
            .Where(pl => pl.Status == PriceListStatus.Active)
            .OrderByDescending(pl => pl.StartDate)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
