using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MilkTea.Domain.Entities.Orders
{
    [Table("OrdersDetail")]
    public class OrdersDetail : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Required, Column("OrderID")]
        public int OrderID { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }

        [Required, Column("MenuID")]
        public int MenuID { get; set; }

        [Required, Column("Quantity")]
        public int Quantity { get; set; }

        [Required, Column("Price")]
        public decimal Price { get; set; }

        [Column("PriceListID")]
        public int? PriceListID { get; set; }

        [Required, Column("CreatedBy")]
        public int CreatedBy { get; set; }

        [Required, Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("CancelledBy")]
        public int? CancelledBy { get; set; }

        [Column("CancelledDate")]
        public DateTime? CancelledDate { get; set; }

        [Column("Note")]
        public string? Note { get; set; }

        [Column("KindOfHotpot1ID")]
        public int? KindOfHotpot1ID { get; set; }

        [Column("KindOfHotpot2ID")]
        public int? KindOfHotpot2ID { get; set; }

        [Required, Column("SizeID")]
        public int SizeID { get; set; }
    }

}
