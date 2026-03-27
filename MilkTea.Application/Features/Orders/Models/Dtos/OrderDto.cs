namespace MilkTea.Application.Features.Orders.Models.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int DinnerTableId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? OrderBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }

        public int? ActionBy { get; set; }
        public DateTime? ActionDate { get; set; }

        public int? CancelledBy { get; set; }
        public DateTime? CancelledDate { get; set; }

        public int? PaymentBy { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? PaymentAmount { get; set; }
        public string? PaymentMethod { get; set; }

        public DateTime? CancellDate { get; set; }
        public string? Note { get; set; }
        public decimal TotalAmount { get; set; }
        public TableDto? DinnerTable { get; set; }
        public OrderStatusDto? Status { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }

    public sealed class OrderStatusDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}

