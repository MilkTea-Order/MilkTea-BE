using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Users.Abstractions.Services;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.User.Services
{
    public class UserService(AppDbContext context) : IUserServices
    {
        private readonly AppDbContext _vContext = context;
        public async Task<bool> isExist(int userID, CancellationToken cancellationToken = default)
        {
            return await _vContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userID, cancellationToken) is not null;
        }
    }
}
