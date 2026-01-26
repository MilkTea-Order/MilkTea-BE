using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Users.Entities;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Identity;

/// <summary>
/// Repository implementation for refresh token operations.
/// </summary>
public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc/>
    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    /// <inheritdoc/>
    public async Task<RefreshToken?> GetValidTokenByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == token 
                && !rt.IsRevoked 
                && rt.ExpiryDate > DateTime.UtcNow);
    }

    /// <inheritdoc/>
    public async Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(int userId)
    {
        return await _context.RefreshTokens
            .AsNoTracking()
            .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.ExpiryDate > DateTime.UtcNow)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
        return refreshToken;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Update(refreshToken);
        return await _context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc/>
    public async Task<bool> RevokeAllUserTokensAsync(int userId)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.Revoke(userId);
        }

        return await _context.SaveChangesAsync() > 0;
    }
}
