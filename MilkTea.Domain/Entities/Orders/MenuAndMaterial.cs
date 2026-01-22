namespace MilkTea.Domain.Entities.Orders
{
    public class MenuAndMaterial : BaseModel
    {
        public int ID { get; set; }
        public int MenuID { get; set; }
        public int SizeID { get; set; }
        public int MaterialsID { get; set; }
        public decimal Quantity { get; set; }

        // Navigations
        public Menu? Menu { get; set; }
        public Material? Material { get; set; }
        public Size? Size { get; set; }
    }
}
