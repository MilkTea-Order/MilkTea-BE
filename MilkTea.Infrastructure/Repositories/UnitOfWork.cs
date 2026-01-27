using Microsoft.EntityFrameworkCore.Storage;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Domain.Configuration.Repositories;
using MilkTea.Domain.Inventory.Repositories;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.Pricing.Repositories;
using MilkTea.Domain.SharedKernel.Repositories;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation that manages database transactions and provides access to repositories.
/// </summary>
public class UnitOfWork(
    AppDbContext context,
    IUserRepository users,
    IEmployeeRepository employees,
    IPermissionRepository permissions,
    IGenderRepository genders,
    IOrderRepository orders,
    IMenuRepository menus,
    ISizeRepository sizes,
    IDinnerTableRepository dinnerTables,
    ITableRepository tables,
    IDefinitionRepository definitions,
    IPriceListRepository priceLists,
    IWarehouseRepository warehouses) : IUnitOfWork
{
    private readonly AppDbContext _context = context;
    private IDbContextTransaction? _transaction;

    public IUserRepository Users { get; } = users;
    public IEmployeeRepository Employees { get; } = employees;
    public IPermissionRepository Permissions { get; } = permissions;
    public IGenderRepository Genders { get; } = genders;
    public IOrderRepository Orders { get; } = orders;
    public IMenuRepository Menus { get; } = menus;
    public ISizeRepository Sizes { get; } = sizes;
    public IDinnerTableRepository DinnerTables { get; } = dinnerTables;
    public ITableRepository Tables { get; } = tables;
    public IDefinitionRepository Definitions { get; } = definitions;
    public IPriceListRepository PriceLists { get; } = priceLists;
    public IWarehouseRepository Warehouses { get; } = warehouses;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            if (_transaction != null)
                await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            Console.WriteLine("Rolling back transaction...");
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}
