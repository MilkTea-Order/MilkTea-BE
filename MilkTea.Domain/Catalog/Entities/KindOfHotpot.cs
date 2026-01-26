using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Catalog.Entities;

/// <summary>
/// Kind of hotpot entity.
/// </summary>
public class KindOfHotpot : Entity<int>
{
    public string Name { get; set; } = null!;
    public string? Note { get; set; }
}
