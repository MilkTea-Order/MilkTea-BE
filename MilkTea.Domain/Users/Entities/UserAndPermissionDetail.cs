namespace MilkTea.Domain.Users.Entities;

public class UserAndPermissionDetail
{
    public int UserID { get; private set; }
    public int PermissionDetailID { get; private set; }
    public int CreatedBy { get; private set; }
    public DateTime CreatedDate { get; private set; }

    private UserAndPermissionDetail() { }

    public static UserAndPermissionDetail Create(int userId, int permissionDetailId, int createdBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(permissionDetailId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        return new UserAndPermissionDetail
        {
            UserID = userId,
            PermissionDetailID = permissionDetailId,
            CreatedBy = createdBy,
            CreatedDate = DateTime.UtcNow
        };
    }
}
