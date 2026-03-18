namespace MilkTea.Application.Features.Inventory.Models.Dtos
{
    public class UnitDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
    }
}
