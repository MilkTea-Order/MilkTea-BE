using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Domain.Users.Entities;

public class Position : EntityId<int>
{
    public string Name { get; set; } = null!;
}
