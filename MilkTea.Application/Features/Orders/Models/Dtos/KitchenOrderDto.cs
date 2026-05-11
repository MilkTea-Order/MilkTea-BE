namespace MilkTea.Application.Features.Orders.Models.Dtos;

public sealed class KitchenOrderDto
{
    public int OrderId { get; set; }
    public TableDto? DinnerTable { get; set; }
    public OrderStatusDto? Status { get; set; }
    public List<KitchenOrderItemDto> OrderItems { get; set; } = new();
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public string? Note { get; set; }
}
public sealed class KitchenOrderItemDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public MenuDto? Menu { get; set; }
    public SizeDto? Size { get; set; }
    public int Quantity { get; set; }
    public OrderStatusDto? Status { get; set; }
    public string? Note { get; set; }
    public int? KindOfHotpot1Id { get; set; }
    public int? KindOfHotpot2Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? PerformDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime? CancelledDate { get; set; }
}
