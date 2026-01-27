using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Users.Entities;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Identity;

/// <summary>
/// Entity Framework Core implementation of <see cref="IPermissionRepository"/>.
/// Provides data access operations for Permission and PermissionGroup entities using EF Core.
/// Handles complex queries for user permissions including both direct assignments and role-based permissions.
/// </summary>
public class PermissionRepository(AppDbContext context) : IPermissionRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc/>
    /// <remarks>
    /// Retrieves all permissions assigned to a user through two paths:
    /// 1. Direct permission assignments via UserPermissions junction table
    /// 2. Indirect permissions through roles via UserRoles -> RoleDetails -> PermissionDetails
    /// Uses LINQ query syntax for complex joins and filtering.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// Returns distinct results to avoid duplicates when user has both direct and role-based permissions.
    /// </remarks>
    public async Task<List<(PermissionDetail PermissionDetail, Permission Permission)>> GetPermissionsByUserIdAsync(int userId)
    {
        var query = from pd in _context.PermissionDetails
                    join p in _context.Permissions on pd.PermissionID equals p.Id
                    where _context.UserPermissions.Any(up => up.UserID == userId && up.PermissionDetailID == pd.Id)
                       || _context.UserRoles
                           .Where(ur => ur.UserID == userId)
                           .Join(_context.RoleDetails,
                               ur => ur.RoleID,
                               rd => rd.RoleID,
                               (ur, rd) => rd.PermissionDetailID)
                           .Contains(pd.Id)
                    select new { PermissionDetail = pd, Permission = p };

        var results = await query
            .AsNoTracking()
            .Distinct()
            .ToListAsync();

        return results
            .Select(r => (r.PermissionDetail, r.Permission))
            .ToList();
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Retrieves all available permissions from the database.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// </remarks>
    public async Task<List<Permission>> GetAllAsync()
    {
        return await _context.Permissions
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Retrieves all permission groups from the database.
    /// Permission groups are used to organize permissions hierarchically.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// </remarks>
    public async Task<List<PermissionGroup>> GetPermissionGroupsAsync()
    {
        return await _context.PermissionGroups
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Retrieves multiple permissions by their IDs in a single query.
    /// Returns a dictionary keyed by permission ID for efficient lookup.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// Returns empty dictionary if no IDs provided or no permissions found.
    /// </remarks>
    public async Task<Dictionary<int, Permission>> GetPermissionsByIdsAsync(IEnumerable<int> permissionIds)
    {
        var ids = permissionIds.ToList();
        if (ids.Count == 0)
            return new Dictionary<int, Permission>();

        var permissions = await _context.Permissions
            .AsNoTracking()
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();

        return permissions.ToDictionary(p => p.Id);
    }
}
