namespace MilkTea.Domain.Entities.Orders
{
    public class PriceListDetail : BaseModel
    {
        public int PriceListID { get; set; }
        public int MenuID { get; set; }
        public decimal Price { get; set; }
        public int SizeID { get; set; }

        // Navigations
        public PriceList? PriceList { get; set; }
        public Menu? Menu { get; set; }
        public Size? Size { get; set; }
    }
}
