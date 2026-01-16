using MilkTea.Domain.Entities.Users;

namespace MilkTea.Domain.Respositories.Users
{
    public interface IRefreshTokenRepository
    {
        Task StoreRefreshTokenAsync(RefreshToken token);
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task RevokeAsync(RefreshToken token);
        Task RevokeAllByUserAsync(int userId);
    }
}
