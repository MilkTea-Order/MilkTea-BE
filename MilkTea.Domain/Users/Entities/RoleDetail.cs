namespace MilkTea.Domain.Users.Entities;

/// <summary>
/// Role detail entity linking roles to permissions.
/// </summary>
public class RoleDetail
{
    public int RoleID { get; private set; }
    public int PermissionDetailID { get; private set; }

    public int CreatedBy { get; private set; }
    public DateTime CreatedDate { get; private set; }

    private RoleDetail() { }

    public static RoleDetail Create(int roleId, int permissionDetailId, int createdBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(roleId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(permissionDetailId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        return new RoleDetail
        {
            RoleID = roleId,
            PermissionDetailID = permissionDetailId,
            CreatedBy = createdBy,
            CreatedDate = DateTime.UtcNow
        };
    }
}
