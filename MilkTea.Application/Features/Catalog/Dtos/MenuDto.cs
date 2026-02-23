namespace MilkTea.Application.Features.Catalog.Dtos
{
    public class MenuDto
    {
        public int MenuId { get; set; }
        public string? MenuCode { get; set; }
        public string? MenuName { get; set; }

        public int MenuGroupId { get; set; }
        public string? MenuGroupName { get; set; }

        public int StatusId { get; set; }
        public string? StatusName { get; set; }

        public int UnitId { get; set; }
        public string? UnitName { get; set; }

        public string? Note { get; set; }

        //public List<SizeAndPrice> Sizes { get; set; } = new();
    }

    //public class SizeAndPrice : SizeDto
    //{
    //    public decimal Price { get; set; }
    //    public int CurrencyId { get; set; }
    //    public string? CurrencyName { get; set; }
    //    public string? CurrencyCode { get; set; }
    //}
}
