using MilkTea.Domain.Entities.Users;

namespace MilkTea.Domain.Respositories.Users
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUserName(string userName, int? excludeId = null);

        Task<User?> GetUserByUserID(int userId, int? excludeId = null);

        Task<User> UpdatePasswordAsync(int userId, string newPassword, int? updateByUserId = null);

        Task<User?> GetByIdAsync(int userId);

        Task<User?> GetUserWithEmployeeAsync(int userId);
    }
}
