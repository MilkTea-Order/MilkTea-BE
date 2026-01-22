namespace MilkTea.Domain.Entities.Orders
{
    public class StatusOfDinnerTable : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
    }
}
