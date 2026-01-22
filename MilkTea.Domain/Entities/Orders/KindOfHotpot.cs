namespace MilkTea.Domain.Entities.Orders
{
    public class KindOfHotpot : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
    }
}
