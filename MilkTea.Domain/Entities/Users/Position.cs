namespace MilkTea.Domain.Entities.Users
{
    public class Position : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
    }
}
