namespace MilkTea.Domain.Entities.Orders
{
    public class PromotionOnTotalBill : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }
        public int? ProPercent { get; set; }
        public decimal? ProAmount { get; set; }
        public int StatusID { get; set; }
        public string? Note { get; set; }

        // Navigation
        public StatusOfPromotion? StatusOfPromotion { get; set; }
    }
}
