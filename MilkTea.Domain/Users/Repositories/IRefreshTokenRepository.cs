using MilkTea.Domain.Users.Entities;

namespace MilkTea.Domain.Users.Repositories;

/// <summary>
/// Repository interface for refresh token operations.
/// </summary>
public interface IRefreshTokenRepository
{
    /// <summary>
    /// Gets a refresh token by token value.
    /// </summary>
    Task<RefreshToken?> GetByTokenAsync(string token);

    /// <summary>
    /// Gets a valid (non-revoked, non-expired) refresh token by token value.
    /// </summary>
    Task<RefreshToken?> GetValidTokenByTokenAsync(string token);

    /// <summary>
    /// Gets active refresh tokens for a user.
    /// </summary>
    Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(int userId);

    /// <summary>
    /// Creates a new refresh token.
    /// </summary>
    Task<RefreshToken> CreateAsync(RefreshToken refreshToken);

    /// <summary>
    /// Updates a refresh token.
    /// </summary>
    Task<bool> UpdateAsync(RefreshToken refreshToken);

    /// <summary>
    /// Revokes all tokens for a user.
    /// </summary>
    Task<bool> RevokeAllUserTokensAsync(int userId);
}
