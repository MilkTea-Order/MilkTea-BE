using MilkTea.Domain.SharedKernel.Abstractions;
using MilkTea.Domain.SharedKernel.Enums;

namespace MilkTea.Domain.Users.Entities;

/// <summary>
/// Role entity for user authorization.
/// </summary>
public class Role : Entity<int>
{
    public string Name { get; set; } = null!;
    public string? Note { get; set; }

    /// <summary>
    /// Role status. Maps to StatusID column.
    /// </summary>
    public CommonStatus Status { get; set; }


    // Navigation
    private readonly List<RoleDetail> _vRoleDetails = new();
    public IReadOnlyList<RoleDetail> RoleDetails => _vRoleDetails.AsReadOnly();
}
