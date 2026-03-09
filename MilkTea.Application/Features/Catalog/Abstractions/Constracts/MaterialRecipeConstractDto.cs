namespace MilkTea.Application.Features.Catalog.Abstractions.Constracts
{
    public class RecipeConstractDto
    {
        //public MenuConstractDto Menu { get; set; } = new();
        //public SizeConstractDto Size { get; set; } = new();
        //public List<MaterialRecipeConstractDto> Materials { get; set; } = new();
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }

    }

    //public class MaterialRecipeConstractDto : MaterialConstractDto
    //{
    //    public decimal Quantity { get; set; }
    //}

}
