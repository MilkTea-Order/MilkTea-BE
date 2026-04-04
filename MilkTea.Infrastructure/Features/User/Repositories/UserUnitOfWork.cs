using Microsoft.EntityFrameworkCore.Storage;
using MilkTea.Domain.User.Repositories;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Features.User.Repositories;

public class UserUnitOfWork(AppDbContext context, IEmployeeRepository employees) : IUserUnitOfWork
{
    private readonly AppDbContext _vContext = context;
    private IDbContextTransaction? _vTransaction;

    public IEmployeeRepository Employees { get; } = employees;


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
