using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Users.Entities;

/// <summary>
/// Employee position entity.
/// </summary>
public class Position : EntityId<int>
{
    public string Name { get; set; } = null!;
}
