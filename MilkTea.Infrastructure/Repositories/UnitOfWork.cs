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


public class UnitOfWork(
    AppDbContext context,
    IUserRepository users,
    IEmployeeRepository employees,
    IPermissionRepository permissions,
    IRoleRepository roles,
    IOrderRepository orders,
    IMenuRepository menus,
    ISizeRepository sizes,
    ITableRepository tables,
    IDefinitionRepository definitions,
    IPriceListRepository priceLists,
    IWarehouseRepository warehouses) : IUnitOfWork
{
    private readonly AppDbContext _vContext = context;
    private IDbContextTransaction? _vTransaction;

    public IUserRepository Users { get; } = users;
    public IEmployeeRepository Employees { get; } = employees;
    public IPermissionRepository Permissions { get; } = permissions;
    public IRoleRepository Roles { get; } = roles;
    public IOrderRepository Orders { get; } = orders;
    public IMenuRepository Menus { get; } = menus;
    public ISizeRepository Sizes { get; } = sizes;
    public ITableRepository Tables { get; } = tables;
    public IDefinitionRepository Definitions { get; } = definitions;
    public IPriceListRepository PriceLists { get; } = priceLists;
    public IWarehouseRepository Warehouses { get; } = warehouses;

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
            Console.WriteLine("Rolling back transaction...");
            await _vTransaction.RollbackAsync(cancellationToken);
            await _vTransaction.DisposeAsync();
            _vTransaction = null;
        }
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _vContext.SaveChangesAsync(cancellationToken);
}
