using MilkTea.Application.Models.Catalog;

namespace MilkTea.Application.Models.Orders
{
    public class Order
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

    public sealed class OrderDetail : Order
    {
        public TableDto? DinnerTable { get; set; }
        public OrderStatus? Status { get; set; }
        public List<OrderLine> OrderDetails { get; set; } = new();
    }

    public sealed class OrderLine
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

        public Menu? Menu { get; set; }
        public Size? Size { get; set; }
    }

    public sealed class Menu
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? MenuGroupName { get; set; }
        public string? StatusName { get; set; }
        public string? UnitName { get; set; }
        public string? Note { get; set; }
    }

    public sealed class Size
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? RankIndex { get; set; }
    }



    public sealed class OrderStatus
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}

