using MilkTea.Domain.Entities.Orders;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Results.Orders
{
    public class GetOrdersByOrderByAndStatusResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
    }
}

