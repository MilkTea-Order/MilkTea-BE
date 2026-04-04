using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Auth.Entities;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Features.Auth.Repositoties;

public class PermissionRepository(AppDbContext context) : IPermissionRepository
{
    private readonly AppDbContext _vContext = context;

    public async Task<List<(PermissionDetailEntity PermissionDetail, PermissionEntity Permission)>> GetPermissionsByUserIdAsync(int userId
                                                                                                , CancellationToken cancellationToken = default)
    {
        var query = from pd in _vContext.PermissionDetails
                    join p in _vContext.Permissions on pd.PermissionID equals p.Id
                    where _vContext.UserPermissions.Any(up => up.UserID == userId && up.PermissionDetailID == pd.Id)
                       || _vContext.UserRoles
                           .Where(ur => ur.UserID == userId)
                           .Join(_vContext.RoleDetails,
                               ur => ur.RoleID,
                               rd => rd.RoleID,
                               (ur, rd) => rd.PermissionDetailID)
                           .Contains(pd.Id)
                    select new { PermissionDetail = pd, Permission = p };

        var results = await query
            .AsNoTracking()
            .Distinct()
            .ToListAsync(cancellationToken);

        return results
            .Select(r => (r.PermissionDetail, r.Permission))
            .ToList();
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Retrieves all available permissions from the database.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// </remarks>
    public async Task<List<PermissionEntity>> GetAllAsync()
    {
        return await _vContext.Permissions
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Retrieves all permission groups from the database.
    /// Permission groups are used to organize permissions hierarchically.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// </remarks>
    public async Task<List<PermissionGroupEntity>> GetPermissionGroupsAsync()
    {
        return await _vContext.PermissionGroups
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
    public async Task<Dictionary<int, PermissionEntity>> GetPermissionsByIdsAsync(IEnumerable<int> permissionIds)
    {
        var ids = permissionIds.ToList();
        if (ids.Count == 0)
            return new Dictionary<int, PermissionEntity>();

        var permissions = await _vContext.Permissions
            .AsNoTracking()
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();

        return permissions.ToDictionary(p => p.Id);
    }
}
