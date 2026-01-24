namespace MilkTea.Application.DTOs.Orders
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int DinnerTableId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? OrderBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? StatusId { get; set; }
        public string? Note { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public sealed class OrderDetailDto : OrderDto
    {
        public DinnerTableDto? DinnerTable { get; set; }
        public OrderStatusDto? Status { get; set; }
        public List<OrderLineDto> OrderDetails { get; set; } = new();
    }

    public sealed class OrderLineDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuId { get; set; }
        public int SizeId { get; set; }
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
        public string? MenuGroupName { get; set; }
        public string? StatusName { get; set; }
        public string? UnitName { get; set; }
        public string? Note { get; set; }
    }

    public sealed class SizeDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? RankIndex { get; set; }
    }

    public sealed class DinnerTableDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; }
        public int? NumberOfSeats { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? Note { get; set; }
    }

    public sealed class OrderStatusDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}

