using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Domain.Users.Entities;


public class Gender : EntityId<int>
{
    public string Name { get; set; } = null!;
}
