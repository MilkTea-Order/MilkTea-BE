using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Domain.Configuration.Entities;

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
