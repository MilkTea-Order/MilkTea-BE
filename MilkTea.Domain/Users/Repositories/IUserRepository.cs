using MilkTea.Domain.Users.Entities;

namespace MilkTea.Domain.Users.Repositories;

/// <summary>
/// Repository interface for user operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets a user by ID.
    /// </summary>
    Task<User?> GetByIdAsync(int id);

    /// <summary>
    /// Gets a user by username.
    /// </summary>
    Task<User?> GetByUserNameAsync(string userName);

    /// <summary>
    /// Gets a user with employee profile.
    /// </summary>
    Task<User?> GetWithEmployeeAsync(int userId);

    /// <summary>
    /// Updates a user.
    /// </summary>
    Task<bool> UpdateAsync(User user);

    /// <summary>
    /// Creates a new user.
    /// </summary>
    Task<User> CreateAsync(User user);
}
