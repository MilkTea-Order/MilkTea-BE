using MilkTea.Domain.SharedKernel.Abstractions;
using MilkTea.Domain.SharedKernel.Enums;

namespace MilkTea.Domain.Users.Entities;

/// <summary>
/// Role entity for user authorization.
/// </summary>
public class Role : Aggregate<int>
{
    private readonly List<RoleDetail> _vRoleDetails = new();
    public IReadOnlyList<RoleDetail> RoleDetails => _vRoleDetails.AsReadOnly();

    public string Name { get; private set; } = null!;
    public string? Note { get; private set; }
    public CommonStatus Status { get; private set; }

    private Role() { }

    public void GrantPermission(int permissionDetailId, int grantedBy)
    {
        if (_vRoleDetails.Any(x => x.PermissionDetailID == permissionDetailId))
            return;

        _vRoleDetails.Add(RoleDetail.Create(
            this.Id,
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
        UpdatedDate = DateTime.UtcNow;
    }

}
