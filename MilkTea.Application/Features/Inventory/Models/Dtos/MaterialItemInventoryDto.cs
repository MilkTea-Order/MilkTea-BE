namespace MilkTea.Application.Features.Inventory.Models.Dtos
{
    public class MaterialItemInventoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public UnitDto UnitMin { get; set; } = new();
        public UnitDto UnitMax { get; set; } = new();
        public int StyleQuantity { get; set; }
        public decimal? LatestPriceImport { get; set; }
        public StatusDto Status { get; set; } = new();
    }
}
