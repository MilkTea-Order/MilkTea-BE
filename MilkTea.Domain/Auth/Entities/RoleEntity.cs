using MilkTea.Domain.Common.Abstractions;
using MilkTea.Domain.Common.Enums;

namespace MilkTea.Domain.Auth.Entities;


public class RoleEntity : Aggregate<int>
{
    private readonly List<RoleDetailEntity> _vRoleDetails = new();
    public IReadOnlyList<RoleDetailEntity> RoleDetails => _vRoleDetails.AsReadOnly();

    public string Name { get; private set; } = null!;
    public string? Note { get; private set; }
    public CommonStatus Status { get; private set; }

    private RoleEntity() { }

    public void GrantPermission(int permissionDetailId, int grantedBy)
    {
        if (_vRoleDetails.Any(x => x.PermissionDetailID == permissionDetailId))
            return;

        _vRoleDetails.Add(RoleDetailEntity.Create(
            Id,
            permissionDetailId,
            grantedBy
        ));

        Touch(grantedBy);
    }

    public void RevokePermission(int permissionDetailId, int revokedBy)
    {
        var item = _vRoleDetails.FirstOrDefault(x => x.PermissionDetailID == permissionDetailId);
        if (item is null) return;

        _vRoleDetails.Remove(item);
        Touch(revokedBy);
    }
    private void Touch(int updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.Now;
    }

}
