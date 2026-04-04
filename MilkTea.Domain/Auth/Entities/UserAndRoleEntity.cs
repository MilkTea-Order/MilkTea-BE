namespace MilkTea.Domain.Auth.Entities;

public class UserAndRoleEntity
{
    public int UserID { get; private set; }
    public int RoleID { get; private set; }
    public int CreatedBy { get; private set; }
    public DateTime CreatedDate { get; private set; }


    private UserAndRoleEntity() { }

    public static UserAndRoleEntity Create(int userId, int roleId, int createdBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(roleId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);
        return new UserAndRoleEntity
        {
            UserID = userId,
            RoleID = roleId,
            CreatedBy = createdBy,
            CreatedDate = DateTime.Now
        };
    }

}
