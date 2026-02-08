using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Catalog.Entities.Size;


public sealed class SizeEntity : Aggregate<int>
{
    public string Name { get; private set; } = null!;
    public int RankIndex { get; private set; }
}
