using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Orders
{
    [Table("warehouserollback")]
    public class WarehouseRollback : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Required, Column("OrderID")]
        public int OrderID { get; set; }

        [Required, Column("WarehouseID")]
        public int WarehouseID { get; set; }

        [Required, Column("QuantitySubtract")]
        public decimal QuantitySubtract { get; set; }

        public Order? Order { get; set; }
        public Warehouse? Warehouse { get; set; }
    }


}
