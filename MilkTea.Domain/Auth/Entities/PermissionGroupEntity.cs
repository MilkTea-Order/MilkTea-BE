using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Domain.Auth.Entities;


public class PermissionGroupEntity : EntityId<int>
{
    private readonly List<PermissionEntity> _vPermissions = new();
    public IReadOnlyList<PermissionEntity> Permissions => _vPermissions.AsReadOnly();
    public string Name { get; set; } = null!;
    public int PermissionGroupTypeID { get; set; }
    public string? Note { get; set; }
}
