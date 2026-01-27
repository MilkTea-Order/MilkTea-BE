using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Users.Entities;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Identity;

/// <summary>
/// Entity Framework Core implementation of <see cref="IUserRepository"/>.
/// Provides read-only query operations for User aggregate using EF Core.
/// Following DDD principles, write operations are handled through UnitOfWork pattern.
/// </summary>
public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc/>
    /// <remarks>
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// Entity is not tracked by EF Core change tracker.
    /// </remarks>
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Searches by UserName value object's internal value property.
    /// Uses AsNoTracking() for read-only queries.
    /// Entity is not tracked by EF Core change tracker.
    /// </remarks>
    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName.value == userName);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Gets a user by ID with change tracking enabled.
    /// Entity is tracked by EF Core change tracker for updates.
    /// Use this method when you need to modify and save the entity.
    /// </remarks>
    public async Task<User?> GetByIdForUpdateAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Gets a user by username with change tracking enabled.
    /// Entity is tracked by EF Core change tracker for updates.
    /// Use this method when you need to modify and save the entity.
    /// </remarks>
    public async Task<User?> GetByUserNameForUpdateAsync(string userName)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserName.value == userName);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Queries RefreshToken child entity through User aggregate.
    /// Uses AsNoTracking() for read-only queries.
    /// RefreshToken is a child entity of User aggregate.
    /// </remarks>
    public async Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Queries valid RefreshToken child entity through User aggregate.
    /// A token is valid if not revoked and not expired.
    /// Uses AsNoTracking() for read-only queries.
    /// </remarks>
    public async Task<RefreshToken?> GetValidRefreshTokenByTokenAsync(string token)
    {
        var now = DateTime.UtcNow;
        return await _context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == token 
                && !rt.IsRevoked 
                && rt.ExpiryDate > now);
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
        return await _context.RefreshTokens
            .AsNoTracking()
            .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.ExpiryDate > now)
            .ToListAsync();
    }
}
