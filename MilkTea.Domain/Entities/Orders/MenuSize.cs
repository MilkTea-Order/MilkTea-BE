using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Orders
{
    [Table("menu_size")]
    public class MenuSize : BaseModel
    {
        [Key, Column("MenuID", Order = 0)]
        public int MenuID { get; set; }

        [Key, Column("SizeID", Order = 1)]
        public int SizeID { get; set; }

        [Column("CostPrice")]
        public decimal? CostPrice { get; set; }

        [Column("SalePrice")]
        public decimal? SalePrice { get; set; }
    }

}
