using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Users.Entities;

/// <summary>
/// Permission group entity for organizing permissions.
/// </summary>
public class PermissionGroup : EntityId<int>
{
    private readonly List<Permission> _vPermissions = new();
    public IReadOnlyList<Permission> Permissions => _vPermissions.AsReadOnly();
    public string Name { get; set; } = null!;
    public int PermissionGroupTypeID { get; set; }
    public string? Note { get; set; }
}
