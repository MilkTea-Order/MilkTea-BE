namespace MilkTea.Domain.Entities.Orders
{
    public class OrdersDetail : BaseModel
    {
        public int ID { get; set; }
        public int OrderID { get; set; }

        public Order? Order { get; set; }

        public int MenuID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int? PriceListID { get; set; }
        public PriceList? PriceList { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CancelledBy { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string? Note { get; set; }
        public int? KindOfHotpot1ID { get; set; }
        public int? KindOfHotpot2ID { get; set; }
        public KindOfHotpot? KindOfHotpot1 { get; set; }
        public KindOfHotpot? KindOfHotpot2 { get; set; }
        public int SizeID { get; set; }
        public Size? Size { get; set; }
        public Menu? Menu { get; set; }
    }
}
