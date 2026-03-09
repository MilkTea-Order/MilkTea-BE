using MilkTea.Domain.Catalog.Menu.Repositories;
using MilkTea.Domain.Catalog.Price.Repositories;
using MilkTea.Domain.Catalog.Size.Repositories;
using MilkTea.Domain.Catalog.Table.Repositories;
using MilkTea.Domain.Configuration.Repositories;
using MilkTea.Domain.Inventory.Repositories;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.Users.Repositories;

namespace MilkTea.Domain.SharedKernel.Repositories;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IEmployeeRepository Employees { get; }
    IPermissionRepository Permissions { get; }
    IRoleRepository Roles { get; }
    IOrderRepository Orders { get; }
    IMenuRepository Menus { get; }
    ISizeRepository Sizes { get; }
    ITableRepository Tables { get; }
    IDefinitionRepository Definitions { get; }
    IPriceListRepository PriceLists { get; }
    IWarehouseRepository Warehouses { get; }


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
