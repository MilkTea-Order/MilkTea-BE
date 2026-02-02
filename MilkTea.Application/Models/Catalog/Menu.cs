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

        public string? UnitName { get; set; }
        public string? Note { get; set; }
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
    public sealed class TableDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; }
        public int? NumberOfSeats { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? Note { get; set; }
        public string? Img { get; set; }
        public string? EmptyImg { get; set; }
        public string? UsingImg { get; set; }
    }
}
