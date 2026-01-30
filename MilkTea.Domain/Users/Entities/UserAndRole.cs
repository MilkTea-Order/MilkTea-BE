namespace MilkTea.Domain.Users.Entities;

/// <summary>
/// Persistence-only junction entity for User-Role many-to-many relationship.
/// This is NOT a domain entity and exists only for EF Core mapping.
/// </summary>
public class UserAndRole
{
    public int UserID { get; private set; }
    public int RoleID { get; private set; }
    public int CreatedBy { get; private set; }
    public DateTime CreatedDate { get; private set; }


    private UserAndRole() { }

    public static UserAndRole Create(int userId, int roleId, int createdBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(roleId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);
        return new UserAndRole
        {
            UserID = userId,
            RoleID = roleId,
            CreatedBy = createdBy,
            CreatedDate = DateTime.UtcNow
        };
    }

}
