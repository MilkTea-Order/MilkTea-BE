using Microsoft.EntityFrameworkCore.Storage;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Catalog;

/// <summary>
/// Unit of Work implementation for Catalog module.
/// </summary>
public class CatalogUnitOfWork(
    AppDbContext context,
    IMenuRepository menus,
    ISizeRepository sizes,
    ITableRepository tables,
    IPriceListRepository priceLists,
    IUnitRepository unit
    //IPromotionRepository promotions
    ) : ICatalogUnitOfWork
{
    private readonly AppDbContext _vContext = context;
    private IDbContextTransaction? _vTransaction;

    public IMenuRepository Menus { get; } = menus;
    public ISizeRepository Sizes { get; } = sizes;
    public ITableRepository Tables { get; } = tables;
    public IPriceListRepository PriceLists { get; } = priceLists;
    public IUnitRepository Unit { get; } = unit;
    //public IPromotionRepository Promotions { get; } = promotions;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _vTransaction = await _vContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _vContext.SaveChangesAsync(cancellationToken);
            if (_vTransaction != null)
                await _vTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_vTransaction != null)
            {
                await _vTransaction.DisposeAsync();
                _vTransaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_vTransaction != null)
        {
            await _vTransaction.RollbackAsync(cancellationToken);
            await _vTransaction.DisposeAsync();
            _vTransaction = null;
        }
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _vContext.SaveChangesAsync(cancellationToken);
}
