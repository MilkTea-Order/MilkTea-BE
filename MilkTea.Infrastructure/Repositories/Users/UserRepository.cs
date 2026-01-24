using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Entities.Users;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Infrastructure.Persistence;
using MilkTea.Shared.Extensions;

namespace MilkTea.Infrastructure.Repositories.Users
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly AppDbContext _vContext = context;

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _vContext.Users.FirstOrDefaultAsync(x => x.ID == userId);
        }

        public async Task<User?> GetUserByUserID(int userId, int? excludeId = null)
        {
            return await _vContext.Users.AsNoTracking().FirstOrDefaultAsync(x =>

                (x.ID == userId) && (excludeId == null || x.ID != excludeId)
              );
        }

        public async Task<User?> GetUserByUserName(string userName, int? excludeId = null)
        {
            return await _vContext.Users.AsNoTracking().FirstOrDefaultAsync(x =>

             (userName.IsNullOrWhiteSpace() || x.UserName == userName)

             && (excludeId == null || x.ID != excludeId)
             );
        }

        public async Task<User> UpdatePasswordAsync(int userId, string newPassword, int? updateByUserId = null)
        {
            var user = await _vContext.Users.FirstOrDefaultAsync(x => x.ID == userId);
            if (user == null) throw new Exception($"User {userId} not found");
            user.Password = newPassword;
            user.PasswordResetBy = updateByUserId;
            user.PasswordResetDate = DateTime.UtcNow;
            user.LastUpdatedBy = updateByUserId;
            user.LastUpdatedDate = DateTime.UtcNow;
            return user;
        }

        public async Task<User?> GetUserWithEmployeeAsync(int userId)
        {
            return await _vContext.Users
                .AsNoTracking()
                .Include(u => u.Employee)
                    .ThenInclude(e => e!.Gender)
                .Include(u => u.Employee)
                    .ThenInclude(e => e!.Position)
                .Include(u => u.Employee)
                    .ThenInclude(e => e!.Status)
                .Include(u => u.Status)
                .FirstOrDefaultAsync(x => x.ID == userId);
        }

    }
}
