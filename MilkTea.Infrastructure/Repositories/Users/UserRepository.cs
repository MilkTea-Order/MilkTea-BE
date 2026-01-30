using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Users.Entities;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Users;


public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _vContext = context;

    /// <inheritdoc/>
    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _vContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _vContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName.value == userName);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Gets a user by ID with change tracking enabled.
    /// Entity is tracked by EF Core change tracker for updates.
    /// Use this method when you need to modify and save the entity.
    /// </remarks>
    public async Task<User?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _vContext.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Gets a user by username with change tracking enabled.
    /// Entity is tracked by EF Core change tracker for updates.
    /// Use this method when you need to modify and save the entity.
    /// </remarks>
    public async Task<User?> GetByUserNameForUpdateAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await _vContext.Users
            .FirstOrDefaultAsync(u => u.UserName.value == userName, cancellationToken);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Queries RefreshToken child entity through User aggregate.
    /// Uses AsNoTracking() for read-only queries.
    /// RefreshToken is a child entity of User aggregate.
    /// </remarks>
    public async Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token)
    {
        return await _vContext.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Queries valid RefreshToken child entity through User aggregate.
    /// A token is valid if not revoked and not expired.
    /// Uses AsNoTracking() for read-only queries.
    /// </remarks>
    public async Task<RefreshToken?> GetValidRefreshTokenByTokenAsync(string token, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        return await _vContext.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == token
                && !rt.IsRevoked
                && rt.ExpiryDate > now, cancellationToken);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Queries active RefreshToken child entities through User aggregate.
    /// Active tokens are non-revoked and non-expired.
    /// Uses AsNoTracking() for read-only queries.
    /// </remarks>
    public async Task<List<RefreshToken>> GetActiveRefreshTokensByUserIdAsync(int userId)
    {
        var now = DateTime.UtcNow;
        return await _vContext.RefreshTokens
            .AsNoTracking()
            .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.ExpiryDate > now)
            .ToListAsync();
    }
}
