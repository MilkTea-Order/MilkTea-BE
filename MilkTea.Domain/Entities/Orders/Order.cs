using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Orders
{
    [Table("Orders")]
    public class Order : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Required, Column("DinnerTableID")]
        public int DinnerTableID { get; set; }

        [Required, Column("OrderBy")]
        public int OrderBy { get; set; }

        [Required, Column("OrderDate")]
        public DateTime OrderDate { get; set; }

        [Required, Column("CreatedBy")]
        public int CreatedBy { get; set; }

        [Required, Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [Column("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [Column("CancelledBy")]
        public int? CancelledBy { get; set; }

        [Column("CancelledDate")]
        public DateTime? CancelledDate { get; set; }

        [Required, Column("StatusOfOrderID")]
        public int StatusOfOrderID { get; set; }

        [Column("Note")]
        public string? Note { get; set; }

        [Column("PaymentedBy")]
        public int? PaymentedBy { get; set; }

        [Column("PaymentedDate")]
        public DateTime? PaymentedDate { get; set; }

        [Column("PaymentedTotal")]
        public decimal? PaymentedTotal { get; set; }

        [Column("PaymentedType")]
        public string? PaymentedType { get; set; }

        [Column("AddNoteBy")]
        public int? AddNoteBy { get; set; }

        [Column("AddNoteDate")]
        public DateTime? AddNoteDate { get; set; }

        [Column("ChangeBy")]
        public int? ChangeBy { get; set; }

        [Column("ChangeDate")]
        public DateTime? ChangeDate { get; set; }

        [Column("MergedBy")]
        public int? MergedBy { get; set; }

        [Column("MergedDate")]
        public DateTime? MergedDate { get; set; }

        [Column("BillNo")]
        public string? BillNo { get; set; }

        [Column("PromotionID")]
        public int? PromotionID { get; set; }

        [Column("PromotionPercent")]
        public int? PromotionPercent { get; set; }

        [Column("PromotionAmount")]
        public decimal? PromotionAmount { get; set; }

        [Column("TotalAmount")]
        public decimal? TotalAmount { get; set; }

        [Column("ActionBy")]
        public int? ActionBy { get; set; }

        [Column("ActionDate")]
        public DateTime? ActionDate { get; set; }

        public ICollection<OrdersDetail> OrdersDetails { get; set; } = new List<OrdersDetail>();
    }


}
