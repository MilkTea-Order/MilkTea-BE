namespace MilkTea.Domain.Entities.Orders
{
    public class StatusOfPriceList : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
    }
}
