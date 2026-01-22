namespace MilkTea.Domain.Entities.Orders
{
    public class WarehouseRollback : BaseModel
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int WarehouseID { get; set; }
        public decimal QuantitySubtract { get; set; }

        public Order? Order { get; set; }
        public Warehouse? Warehouse { get; set; }
    }
}
