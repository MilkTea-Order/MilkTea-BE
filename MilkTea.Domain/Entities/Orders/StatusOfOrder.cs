namespace MilkTea.Domain.Entities.Orders
{
    public class StatusOfOrder : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
    }
}
