using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Configuration.Entities;

/// <summary>
/// Definition entity for system configuration.
/// </summary>
public class Definition : Entity<int>
{
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string? Value { get; set; }
    public byte[]? ValueImage { get; set; }
    public int IsEdit { get; set; }
    public int IsEncrypt { get; set; }
    public int DefinitionGroupID { get; set; }


}
