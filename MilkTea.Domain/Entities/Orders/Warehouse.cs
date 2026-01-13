using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Orders
{
    [Table("Warehouse")]
    public class Warehouse : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Required, Column("MaterialsID")]
        public int MaterialsID { get; set; }

        [Required, Column("QuantityImport")]
        public decimal QuantityImport { get; set; }

        [Required, Column("QuantityCurrent")]
        public decimal QuantityCurrent { get; set; }

        [Required, Column("PriceImport")]
        public decimal PriceImport { get; set; }

        [Required, Column("AmountTotal")]
        public decimal AmountTotal { get; set; }

        [Required, Column("ImportFromSuppliersID")]
        public int ImportFromSuppliersID { get; set; }

        [Required, Column("StatusID")]
        public int StatusID { get; set; }
    }

}
