using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Configuration.Entities;

/// <summary>
/// Definition group entity for organizing configuration items.
/// </summary>
public class DefinitionGroup : Aggregate<int>
{
    private readonly List<Definition> _vDenifitions = new();
    public IReadOnlyList<Definition> Denifitions => _vDenifitions.AsReadOnly();

    public string Name { get; set; } = null!;
}
