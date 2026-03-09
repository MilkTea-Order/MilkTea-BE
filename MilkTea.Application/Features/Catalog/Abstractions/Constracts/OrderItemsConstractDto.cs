namespace MilkTea.Application.Features.Catalog.Abstractions.Constracts
{
    public class OrderItemsConstractDto
    {
        public int MenuId { get; set; }
        public int SizeId { get; set; }
        public decimal Quantity { get; set; }
    }
}
