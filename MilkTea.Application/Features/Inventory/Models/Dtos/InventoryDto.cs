namespace MilkTea.Application.Features.Inventory.Models.Dtos
{
    public class InventoryStockDto
    {
        public int MaterialId { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal LatestPriceImport { get; set; }
    }
}
