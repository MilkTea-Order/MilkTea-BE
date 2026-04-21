using MilkTea.Domain.Auth.Entities;

namespace MilkTea.Domain.Auth.Repositories;

public interface ISessionRepository
{
    /// <summary>
    /// Gets a session entity by its ID.
    /// </summary>
    Task<SessionEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a session entity by its ID with tracking enabled for updates.
    /// </summary>
    Task<SessionEntity?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the latest active session for an email address and function.
    /// </summary>
    Task<SessionEntity?> GetLatestActiveSessionByEmailAndFunctionAsync(string email, string function, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the latest session for an email address and function.
    /// </summary>
    Task<SessionEntity?> GetLatestSessionByEmailAndFunctionAsync(string email, string function, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new session entity to the database.
    /// </summary>
    Task AddAsync(SessionEntity session, CancellationToken cancellationToken = default);
}
