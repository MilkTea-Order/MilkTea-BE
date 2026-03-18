namespace MilkTea.Application.Features.Catalog.Models.Dtos.Material
{
    public class MaterialDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<MaterialItemDto> MaterialItems { get; set; } = new();
    }
}
