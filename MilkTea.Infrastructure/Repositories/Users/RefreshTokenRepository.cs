using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Entities.Users;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Users
{
    public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
    {
        private readonly AppDbContext _vContext = context;
        public async Task StoreRefreshTokenAsync(RefreshToken token)
        {
            await _vContext.RefreshTokens.AddAsync(token);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _vContext.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task<RefreshToken?> GetValidTokenByTokenAsync(string token)
        {
            var now = DateTime.UtcNow;
            return await _vContext.RefreshTokens
                .FirstOrDefaultAsync(t =>
                    t.Token == token
                    && !t.IsRevoked
                    && t.ExpiryDate > now);
        }
        public async Task<RefreshToken?> GetTokenAndRevokeAsync(string token)
        {
            var now = DateTime.UtcNow;

            var refreshToken = await _vContext.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token);

            if (refreshToken == null) return null;

            if (!refreshToken.IsRevoked)
            {
                refreshToken.IsRevoked = true;
                refreshToken.LastUpdatedDate = now;
            }
            return refreshToken;
        }

        public async Task RevokeAsync(RefreshToken token)
        {
            token.IsRevoked = true;
            token.LastUpdatedDate = DateTime.UtcNow;
            await Task.CompletedTask;
        }

        public async Task RevokeAllByUserAsync(int userId)
        {
            var tokens = _vContext.RefreshTokens
                .Where(t => t.UserId == userId && !t.IsRevoked)
                .ToList();

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.LastUpdatedDate = DateTime.UtcNow;
            }

            await Task.CompletedTask;
        }
    }
}
