namespace MilkTea.Infrastructure.Persistence.Entities;

/// <summary>
/// Persistence-only junction entity for User-PermissionDetail many-to-many relationship.
/// This is NOT a domain entity and exists only for EF Core mapping.
/// </summary>
public class UserAndPermissionDetail
{
    public int UserID { get; set; }
    public int PermissionDetailID { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}
