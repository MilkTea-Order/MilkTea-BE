using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Users.Entities;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Identity;

/// <summary>
/// Repository implementation for permission operations.
/// </summary>
public class PermissionRepository(AppDbContext context) : IPermissionRepository
{
    private readonly AppDbContext _vContext = context;

    /// <inheritdoc/>
    public async Task<List<(PermissionDetail PermissionDetail, Permission Permission)>> GetPermissionsByUserIdAsync(int userId)
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
            .ToListAsync();

        return results
            .Select(r => (r.PermissionDetail, r.Permission))
            .ToList();
    }

    /// <inheritdoc/>
    public async Task<List<Permission>> GetAllAsync()
    {
        return await _vContext.Permissions
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<PermissionGroup>> GetPermissionGroupsAsync()
    {
        return await _vContext.PermissionGroups
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Dictionary<int, Permission>> GetPermissionsByIdsAsync(IEnumerable<int> permissionIds)
    {
        var ids = permissionIds.ToList();
        if (ids.Count == 0)
            return new Dictionary<int, Permission>();

        var permissions = await _vContext.Permissions
            .AsNoTracking()
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();

        return permissions.ToDictionary(p => p.Id);
    }
}
