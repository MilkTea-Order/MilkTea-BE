namespace MilkTea.Domain.Entities.Orders
{
    public class Size : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public int RankIndex { get; set; }
    }
}
