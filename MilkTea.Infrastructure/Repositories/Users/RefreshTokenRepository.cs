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


    }
}
