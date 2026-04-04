using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Domain.Catalog.Price.Entities;

public class CurrencyEntity : EntityId<int>
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
