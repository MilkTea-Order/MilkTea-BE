using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Catalog.Material.Entities;

public class MaterialsGroupEntity : EntityId<int>
{
    public string Name { get; set; } = "";
}
