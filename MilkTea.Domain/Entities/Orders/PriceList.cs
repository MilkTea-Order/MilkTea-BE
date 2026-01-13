using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Orders
{
    [Table("pricelist")]
    public class PriceList : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Required, Column("Name")]
        public string Name { get; set; } = null!;

        [Column("Code")]
        public string? Code { get; set; }

        [Required, Column("StartDate")]
        public DateTime StartDate { get; set; }

        [Required, Column("StopDate")]
        public DateTime StopDate { get; set; }

        [Required, Column("CurrencyID")]
        public int CurrencyID { get; set; }

        [Required, Column("StatusOfPriceListID")]
        public int StatusOfPriceListID { get; set; }
    }


}
