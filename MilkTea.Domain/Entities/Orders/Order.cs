namespace MilkTea.Domain.Entities.Orders
{
    public class Order : BaseModel
    {
        public int ID { get; set; }
        public int DinnerTableID { get; set; }
        public int OrderBy { get; set; }
        public DateTime OrderDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? CancelledBy { get; set; }
        public DateTime? CancelledDate { get; set; }
        public int StatusOfOrderID { get; set; }
        public string? Note { get; set; }
        public int? PaymentedBy { get; set; }
        public DateTime? PaymentedDate { get; set; }
        public decimal? PaymentedTotal { get; set; }
        public string? PaymentedType { get; set; }
        public int? AddNoteBy { get; set; }
        public DateTime? AddNoteDate { get; set; }
        public int? ChangeBy { get; set; }
        public DateTime? ChangeDate { get; set; }
        public int? MergedBy { get; set; }
        public DateTime? MergedDate { get; set; }
        public string? BillNo { get; set; }
        public int? PromotionID { get; set; }
        public int? PromotionPercent { get; set; }
        public decimal? PromotionAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? ActionBy { get; set; }
        public DateTime? ActionDate { get; set; }

        // Navigations
        public DinnerTable? DinnerTable { get; set; }
        public StatusOfOrder? StatusOfOrder { get; set; }
        public PromotionOnTotalBill? PromotionOnTotalBill { get; set; }

        // Last
        public ICollection<OrdersDetail> OrdersDetails { get; set; } = new List<OrdersDetail>();
    }
}
