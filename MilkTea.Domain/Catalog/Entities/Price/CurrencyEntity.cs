using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Catalog.Entities.Price;

public class CurrencyEntity : EntityId<int>
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
