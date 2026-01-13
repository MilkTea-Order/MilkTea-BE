using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Orders
{
    [Table("PriceListDetail")]
    public class PriceListDetail : BaseModel
    {
        [Key, Column("PriceListID", Order = 0)]
        public int PriceListID { get; set; }

        [Key, Column("MenuID", Order = 1)]
        public int MenuID { get; set; }

        [Required, Column("Price")]
        public decimal Price { get; set; }

        [Key, Column("SizeID", Order = 2)]
        public int SizeID { get; set; }
    }


}
