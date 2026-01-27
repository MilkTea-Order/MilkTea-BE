using MilkTea.Domain.Users.Entities;

namespace MilkTea.Domain.Users.Repositories;

/// <summary>
/// Repository interface for Permission and PermissionGroup entity operations.
/// Provides methods to query permission-related data for authorization purposes.
/// </summary>
public interface IPermissionRepository
{
    /// <summary>
    /// Gets all permissions assigned to a specific user.
    /// Returns both PermissionDetail (user-permission relationship) and Permission (permission definition).
    /// </summary>
    /// <param name="userId">The user ID to get permissions for.</param>
    /// <returns>A list of tuples containing PermissionDetail and Permission for each assigned permission.</returns>
    Task<List<(PermissionDetail PermissionDetail, Permission Permission)>> GetPermissionsByUserIdAsync(int userId);

    /// <summary>
    /// Gets all available permissions from the database.
    /// </summary>
    /// <returns>A list of all permissions.</returns>
    Task<List<Permission>> GetAllAsync();

    /// <summary>
    /// Gets all permission groups from the database.
    /// Permission groups are used to organize permissions hierarchically.
    /// </summary>
    /// <returns>A list of all permission groups.</returns>
    Task<List<PermissionGroup>> GetPermissionGroupsAsync();

    /// <summary>
    /// Gets multiple permissions by their IDs in a single query.
    /// Returns a dictionary keyed by permission ID for efficient lookup.
    /// </summary>
    /// <param name="permissionIds">The collection of permission IDs to retrieve.</param>
    /// <returns>A dictionary mapping permission ID to Permission entity.</returns>
    Task<Dictionary<int, Permission>> GetPermissionsByIdsAsync(IEnumerable<int> permissionIds);
}
