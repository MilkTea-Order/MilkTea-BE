namespace MilkTea.Domain.Entities.Orders
{
    public class Material : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public int? UnitID { get; set; }
        public int? UnitID_Max { get; set; }
        public int? StyleQuantity { get; set; }
        public int MaterialsGroupID { get; set; }
        public int StatusID { get; set; }

        // Navigations
        public MaterialsGroup? MaterialsGroup { get; set; }
        public MaterialsStatus? MaterialsStatus { get; set; }
        public Unit? Unit { get; set; }
        public Unit? UnitMax { get; set; }
    }
}
