namespace MilkTea.Application.Queries.Orders
{
    public sealed class GetOrderDetailByIdAndStatusQuery
    {
        public int OrderId { get; set; }
        public bool? IsCancelled { get; set; }
    }
}

