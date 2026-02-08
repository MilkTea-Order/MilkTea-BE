using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Catalog.Entities.Menu;

public class KindOfHotpot : EntityId<int>
{
    public string Name { get; set; } = null!;
}
