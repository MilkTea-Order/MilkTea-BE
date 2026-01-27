using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Users.Entities;

/// <summary>
/// Permission entity.
/// </summary>
public class Permission : EntityId<int>
{
    private readonly List<PermissionDetail> _vPermissionDetails = new();
    public IReadOnlyList<PermissionDetail> PermissionDetails => _vPermissionDetails.AsReadOnly();
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public int PermissionGroupID { get; set; }
    public string? Note { get; set; }
}
