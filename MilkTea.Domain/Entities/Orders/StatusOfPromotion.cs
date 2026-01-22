namespace MilkTea.Domain.Entities.Orders
{
    public class StatusOfPromotion : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
    }
}
