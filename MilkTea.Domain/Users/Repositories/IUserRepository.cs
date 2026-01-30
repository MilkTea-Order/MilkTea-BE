using MilkTea.Domain.Users.Entities;

namespace MilkTea.Domain.Users.Repositories;

/// <summary>
/// Repository interface for User aggregate operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets a user by their unique identifier (read-only, not tracked).
    /// </summary>
    /// <param name="id">The user ID to search for.</param>
    /// <returns>The user if found, otherwise null.</returns>
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a user by their unique identifier with change tracking enabled.
    /// Use this method when you need to update the entity.
    /// </summary>
    /// <param name="id">The user ID to search for.</param>
    /// <returns>The tracked user if found, otherwise null.</returns>
    Task<User?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a user by their username (read-only, not tracked).
    /// </summary>
    /// <param name="userName">The username to search for.</param>
    /// <returns>The user if found, otherwise null.</returns>
    Task<User?> GetByUserNameAsync(string userName);

    /// <summary>
    /// Gets a user by their username with change tracking enabled.
    /// Use this method when you need to update the entity.
    /// </summary>
    /// <param name="userName">The username to search for.</param>
    /// <returns>The tracked user if found, otherwise null.</returns>
    Task<User?> GetByUserNameForUpdateAsync(string userName, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a refresh token by its token value.
    /// This method queries RefreshToken child entity through User aggregate.
    /// Does not filter by revocation or expiration status.
    /// </summary>
    /// <param name="token">The refresh token string to search for.</param>
    /// <returns>The refresh token with its user if found, otherwise null.</returns>
    Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token);

    /// <summary>
    /// Gets a valid refresh token by its token value.
    /// A token is considered valid if it is not revoked and not expired.
    /// This method queries RefreshToken child entity through User aggregate.
    /// </summary>
    /// <param name="token">The refresh token string to search for.</param>
    /// <returns>The valid refresh token with its user if found, otherwise null.</returns>
    Task<RefreshToken?> GetValidRefreshTokenByTokenAsync(string token, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all active (non-revoked and non-expired) refresh tokens for a specific user.
    /// This method queries RefreshToken child entities through User aggregate.
    /// </summary>
    /// <param name="userId">The user ID to get active tokens for.</param>
    /// <returns>A list of active refresh tokens for the user.</returns>
    Task<List<RefreshToken>> GetActiveRefreshTokensByUserIdAsync(int userId);
}
