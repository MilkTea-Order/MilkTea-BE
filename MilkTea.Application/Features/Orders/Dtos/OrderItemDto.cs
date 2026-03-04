namespace MilkTea.Application.Features.Orders.Dtos
{
    public sealed class OrderItemDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int PriceListId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CancelledBy { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string? Note { get; set; }
        public int? KindOfHotpot1Id { get; set; }
        public int? KindOfHotpot2Id { get; set; }
        public MenuDto? Menu { get; set; }
        public SizeDto? Size { get; set; }
    }

    public sealed class MenuDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? MenuGroupId { get; set; }
        public string? MenuGroupName { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public int? UnitId { get; set; }
        public string? UnitName { get; set; }
        public string? Note { get; set; }
    }

    public sealed class SizeDto

    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? RankIndex { get; set; }
    }
}
