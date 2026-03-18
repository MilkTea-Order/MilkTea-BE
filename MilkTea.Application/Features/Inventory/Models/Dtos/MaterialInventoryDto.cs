namespace MilkTea.Application.Features.Inventory.Models.Dtos
{
    public class MaterialInventoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<MaterialItemInventoryDto> MaterialItems { get; set; } = new();
    }
}
