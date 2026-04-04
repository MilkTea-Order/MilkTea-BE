namespace MilkTea.Domain.Auth.Entities;


public class RoleDetailEntity
{
    public int RoleID { get; private set; }
    public int PermissionDetailID { get; private set; }

    public int CreatedBy { get; private set; }
    public DateTime CreatedDate { get; private set; }

    private RoleDetailEntity() { }

    public static RoleDetailEntity Create(int roleId, int permissionDetailId, int createdBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(roleId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(permissionDetailId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        return new RoleDetailEntity
        {
            RoleID = roleId,
            PermissionDetailID = permissionDetailId,
            CreatedBy = createdBy,
            CreatedDate = DateTime.Now
        };
    }
}
