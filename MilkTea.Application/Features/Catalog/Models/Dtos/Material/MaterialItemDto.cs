using MilkTea.Application.Features.Catalog.Models.Dtos.Unit;

namespace MilkTea.Application.Features.Catalog.Models.Dtos.Material
{
    public class MaterialItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public int StyleQuantity { get; set; }
        public UnitDto UnitMin { get; set; } = new();
        public UnitDto UnitMax { get; set; } = new();
        public StatusDto Status { get; set; } = new();
    }
}
