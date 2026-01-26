using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Configuration.Entities;

/// <summary>
/// Definition group entity for organizing configuration items.
/// </summary>
public class DefinitionGroup : Entity<int>
{
    public string Name { get; set; } = null!;

    // Navigation
    public ICollection<Definition> Definitions { get; set; } = new List<Definition>();
}
