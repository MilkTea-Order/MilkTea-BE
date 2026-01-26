using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Users.Entities;

/// <summary>
/// Permission group type entity.
/// </summary>
public class PermissionGroupType : EntityId<int>
{
    private readonly List<PermissionGroup> _vPermissionGroups = new();
    public IReadOnlyList<PermissionGroup> PermissionGroups => _vPermissionGroups.AsReadOnly();
    public string Name { get; set; } = null!;
    public string? Note { get; set; }
}
