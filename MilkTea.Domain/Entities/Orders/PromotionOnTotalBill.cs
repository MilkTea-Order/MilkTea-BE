using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Orders
{
    [Table("PromotionOnTotalBill")]
    public class PromotionOnTotalBill : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Required, Column("Name")]
        public string Name { get; set; } = null!;

        [Required, Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Required, Column("CreatedBy")]
        public int CreatedBy { get; set; }

        [Column("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [Column("UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [Required, Column("StartDate")]
        public DateTime StartDate { get; set; }

        [Required, Column("StopDate")]
        public DateTime StopDate { get; set; }

        [Column("ProPercent")]
        public int? ProPercent { get; set; }

        [Column("ProAmount")]
        public decimal? ProAmount { get; set; }

        [Required, Column("StatusID")]
        public int StatusID { get; set; }

        [Column("Note")]
        public string? Note { get; set; }
    }


}
