using Microsoft.EntityFrameworkCore.Storage;
using MilkTea.Domain.Configuration.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Configuration;

/// <summary>
/// Unit of Work implementation for Configuration module.
/// </summary>
public class ConfigurationUnitOfWork(
    AppDbContext context,
    IDefinitionRepository definitions) : IConfigurationUnitOfWork
{
    private readonly AppDbContext _vContext = context;
    private IDbContextTransaction? _vTransaction;

    public IDefinitionRepository Definitions { get; } = definitions;

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
