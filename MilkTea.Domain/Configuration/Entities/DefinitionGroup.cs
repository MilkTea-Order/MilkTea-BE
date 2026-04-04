using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Domain.Configuration.Entities;


public class DefinitionGroup : Aggregate<int>
{
    private readonly List<Definition> _vDenifitions = new();
    public IReadOnlyList<Definition> Denifitions => _vDenifitions.AsReadOnly();

    public string Name { get; set; } = null!;
}
