using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Domain.Auth.Entities;

public class PermissionEntity : EntityId<int>
{
    private readonly List<PermissionDetailEntity> _vPermissionDetails = new();
    public IReadOnlyList<PermissionDetailEntity> PermissionDetails => _vPermissionDetails.AsReadOnly();
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public int PermissionGroupID { get; set; }
    public string? Note { get; set; }
}
