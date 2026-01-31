namespace MilkTea.Domain.Users.Repositories;

/// <summary>
/// Unit of Work interface for Users module.
/// Manages transactions and provides access to Users repositories.
/// </summary>
public interface IUserUnitOfWork
{
    IUserRepository Users { get; }
    IEmployeeRepository Employees { get; }
    IRoleRepository Roles { get; }
    IPermissionRepository Permissions { get; }

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
