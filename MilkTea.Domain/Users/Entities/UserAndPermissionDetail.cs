namespace MilkTea.Domain.Users.Entities;

/// <summary>
/// Junction entity linking users directly to permission details.
/// </summary>
public class UserAndPermissionDetail
{
    public int UserID { get; set; }
    public int PermissionDetailID { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}
