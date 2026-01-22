namespace MilkTea.Domain.Entities.Orders
{
    public class MenuSize : BaseModel
    {
        public int MenuID { get; set; }
        public int SizeID { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? SalePrice { get; set; }

        public Menu? Menu { get; set; }
        public Size? Size { get; set; }
    }
}
