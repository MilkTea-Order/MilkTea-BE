using MilkTea.Domain.Entities.Users;

namespace MilkTea.Domain.Entities.Orders
{
    public class Warehouse : BaseModel
    {
        public int ID { get; set; }
        public int MaterialsID { get; set; }
        public decimal QuantityImport { get; set; }
        public decimal QuantityCurrent { get; set; }
        public decimal PriceImport { get; set; }
        public decimal AmountTotal { get; set; }
        public int ImportFromSuppliersID { get; set; }
        public int StatusID { get; set; }

        public Material? Material { get; set; }
        public Status? Status { get; set; }
    }
}
