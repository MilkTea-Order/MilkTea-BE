namespace MilkTea.Domain.Entities.Config
{
    public class Definition : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Value { get; set; }
        public byte[]? ValueImage { get; set; }
        public int IsEdit { get; set; }
        public int IsEncrypt { get; set; }
        public int DefinitionGroupID { get; set; }

        // Navigation
        public DefinitionGroup? DefinitionGroup { get; set; }
    }
}
