namespace MilkTea.Domain.Entities.Users
{
    public class Gender : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
    }
}
