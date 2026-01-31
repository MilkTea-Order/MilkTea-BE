
namespace MilkTea.Domain.Catalog.Repositories;

/// <summary>
/// Unit of Work interface for Catalog module.
/// Manages transactions and provides access to Catalog repositories.
/// </summary>
public interface ICatalogUnitOfWork
{
    IMenuRepository Menus { get; }
    ISizeRepository Sizes { get; }
    ITableRepository Tables { get; }
    IPriceListRepository PriceLists { get; }
    //IPromotionRepository Promotions { get; }

    /// <summary>
    /// Saves all changes made in the current transaction.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
