using MilkTea.Application.Models.Orders;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Results
{
    public class GetOrdersByOrderByAndStatusResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
    }
}

