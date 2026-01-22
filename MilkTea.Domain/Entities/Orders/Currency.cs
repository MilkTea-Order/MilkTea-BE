namespace MilkTea.Domain.Entities.Orders
{
    public class Currency : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}
