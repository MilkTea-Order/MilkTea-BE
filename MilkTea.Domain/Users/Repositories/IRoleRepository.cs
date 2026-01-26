using MilkTea.Domain.Users.Entities;

namespace MilkTea.Domain.Users.Repositories;

/// <summary>
/// Repository interface for role operations.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Gets a role by ID.
    /// </summary>
    Task<Role?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all roles.
    /// </summary>
    Task<List<Role>> GetAllAsync();

    /// <summary>
    /// Gets roles for a user.
    /// </summary>
    Task<List<Role>> GetRolesByUserIdAsync(int userId);
}
