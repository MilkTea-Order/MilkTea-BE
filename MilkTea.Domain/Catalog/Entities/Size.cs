using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Catalog.Entities;


public sealed class Size : Aggregate<int>
{
    public string Name { get; private set; } = null!;
    public int RankIndex { get; private set; }
}
