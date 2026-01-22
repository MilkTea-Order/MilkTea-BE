namespace MilkTea.Domain.Entities.Config
{
    public class DefinitionGroup : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
    }
}
