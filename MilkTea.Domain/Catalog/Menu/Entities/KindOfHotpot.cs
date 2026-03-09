using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Catalog.Menu.Entities;

public class KindOfHotpot : EntityId<int>
{
    public string Name { get; set; } = null!;
}
