using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Users.Entities;

/// <summary>
/// Gender entity.
/// </summary>
public class Gender : EntityId<int>
{
    public string Name { get; set; } = null!;
}
