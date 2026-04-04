using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Domain.Auth.Entities;

public class PermissionDetailEntity : EntityId<int>
{
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public int PermissionID { get; set; }
    public string? Note { get; set; }
}
