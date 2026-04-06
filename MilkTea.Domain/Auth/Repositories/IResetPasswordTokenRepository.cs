using MilkTea.Domain.Auth.Entities;

namespace MilkTea.Domain.Auth.Repositories;

public interface IResetPasswordTokenRepository
{
    /// <summary>
    /// Gets a reset password token entity by its ID.
    /// </summary>
    Task<ResetPasswordTokenEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a reset password token entity by its ID with tracking enabled for updates.
    /// </summary>
    Task<ResetPasswordTokenEntity?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a valid (not used, not expired) reset password token by its token value.
    /// </summary>
    Task<ResetPasswordTokenEntity?> GetValidTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a valid reset password token by its token value with tracking enabled.
    /// </summary>
    Task<ResetPasswordTokenEntity?> GetValidTokenForUpdateAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new reset password token entity to the database.
    /// </summary>
    Task AddAsync(ResetPasswordTokenEntity token, CancellationToken cancellationToken = default);
}
