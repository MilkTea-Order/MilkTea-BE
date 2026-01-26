using MilkTea.Domain.Users.Entities;

namespace MilkTea.Domain.Users.Repositories;

/// <summary>
/// Repository interface for permission operations.
/// </summary>
public interface IPermissionRepository
{
    /// <summary>
    /// Gets permissions for a user with Permission information included.
    /// Returns a tuple of (PermissionDetail, Permission) for each permission.
    /// </summary>
    Task<List<(PermissionDetail PermissionDetail, Permission Permission)>> GetPermissionsByUserIdAsync(int userId);

    /// <summary>
    /// Gets all permissions.
    /// </summary>
    Task<List<Permission>> GetAllAsync();

    /// <summary>
    /// Gets permission groups.
    /// </summary>
    Task<List<PermissionGroup>> GetPermissionGroupsAsync();

    /// <summary>
    /// Gets permissions by their IDs.
    /// </summary>
    Task<Dictionary<int, Permission>> GetPermissionsByIdsAsync(IEnumerable<int> permissionIds);
}
