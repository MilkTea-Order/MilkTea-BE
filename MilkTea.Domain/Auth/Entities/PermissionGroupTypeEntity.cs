using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Domain.Auth.Entities;

public class PermissionGroupTypeEntity : EntityId<int>
{
    private readonly List<PermissionGroupEntity> _vPermissionGroups = new();
    public IReadOnlyList<PermissionGroupEntity> PermissionGroups => _vPermissionGroups.AsReadOnly();
    public string Name { get; set; } = null!;
    public string? Note { get; set; }
}
