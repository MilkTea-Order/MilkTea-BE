using MilkTea.Domain.Users.Entities;

namespace MilkTea.Domain.Users.Repositories;

/// <summary>
/// Repository interface for Role entity operations.
/// Roles are used for role-based access control (RBAC) in the system.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Gets a role by its unique identifier.
    /// </summary>
    /// <param name="id">The role ID to search for.</param>
    /// <returns>The role if found, otherwise null.</returns>
    Task<Role?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all available roles from the database.
    /// </summary>
    /// <returns>A list of all roles.</returns>
    Task<List<Role>> GetAllAsync();

    /// <summary>
    /// Gets all roles assigned to a specific user.
    /// </summary>
    /// <param name="userId">The user ID to get roles for.</param>
    /// <returns>A list of roles assigned to the user.</returns>
    Task<List<Role>> GetRolesByUserIdAsync(int userId);
}
