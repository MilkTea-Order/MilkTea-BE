namespace MilkTea.Application.Models.Catalog
{
    public sealed class MenuGroupDto
    {
        public int MenuGroupId { get; set; }
        public string? MenuGroupName { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public int Quantity { get; set; }
    }

    public sealed class MenuItemDto
    {
        public int MenuId { get; set; }
        public string? MenuCode { get; set; }
        public string? MenuName { get; set; }
        public int MenuGroupId { get; set; }
        public string? MenuGroupName { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
    }

    public class MenuSizeDto
    {
        public int SizeId { get; set; }
        public string? SizeName { get; set; }
        public int RankIndex { get; set; }
    }

    public sealed class MenuSizePriceDto : MenuSizeDto
    {
        public decimal Price { get; set; }
        public string? CurrencyName { get; set; }
        public string? CurrencyCode { get; set; }
    }
}
