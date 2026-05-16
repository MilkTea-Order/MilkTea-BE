using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Auth.Abstractions.Queries;
using MilkTea.Application.Features.Auth.Models.Dtos;
using MilkTea.Application.Ports.Hash.Permission;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Features.Auth.Queries;

public class PermissionQuery(
    AppDbContext context,
    IPermissionHasher permissionHasher) : IPermissionQuery
{
    private readonly AppDbContext _vContext = context;
    private readonly IPermissionHasher _vPermissionHasher = permissionHasher;

    public async Task<List<PermissionDto>> GetPermissionsByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var userPermissionDetailIds = _vContext.UserPermissions
            .AsNoTracking()
            .Where(up => up.UserID == userId)
            .Select(up => up.PermissionDetailID);

        var rolePermissionDetailIds = _vContext.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserID == userId)
            .SelectMany(ur => _vContext.RoleDetails
                .AsNoTracking()
                .Where(rd => rd.RoleID == ur.RoleID)
                .Select(rd => rd.PermissionDetailID));

        var permissionDetailIds = userPermissionDetailIds
            .Union(rolePermissionDetailIds)
            .Distinct();

        var results = await _vContext.PermissionDetails
            .AsNoTracking()
            .Where(pd => permissionDetailIds.Contains(pd.Id))
            .Join(_vContext.Permissions.AsNoTracking(),
                pd => pd.PermissionID,
                p => p.Id,
                (pd, p) => new { Permission = p, PermissionDetail = pd })
            .ToListAsync(cancellationToken);

        return results
            .GroupBy(r => r.Permission.Id)
            .Select(g =>
            {
                var perm = g.First().Permission;
                return new PermissionDto(
                    perm.Id,
                    perm.Name,
                    perm.Code,
                    g.Select(r => new PermissionDetailDto(
                        r.PermissionDetail.Id,
                        r.PermissionDetail.Name,
                        _vPermissionHasher.DecodePermission(r.PermissionDetail.Code),
                        r.PermissionDetail.Note
                    )).ToList()
                );
            })
            .ToList();
    }
}
