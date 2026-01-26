using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Users.Entities;

/// <summary>
/// Role detail entity linking roles to permissions.
/// </summary>
public class RoleDetail : Entity<int>
{
    public int RoleID { get; set; }
    public int PermissionDetailID { get; set; }
}
