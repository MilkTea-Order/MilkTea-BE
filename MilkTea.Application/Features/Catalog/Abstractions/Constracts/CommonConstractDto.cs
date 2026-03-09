namespace MilkTea.Application.Features.Catalog.Abstractions.Constracts
{
    public class MenuConstractDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class SizeConstractDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RankIndex { get; set; }
    }

    public class MaterialConstractDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
