using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Users.Entities;

/// <summary>
/// Permission detail entity with granular access control.
/// </summary>
public class PermissionDetail : EntityId<int>
{
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public int PermissionID { get; set; }
    public string? Note { get; set; }
}
