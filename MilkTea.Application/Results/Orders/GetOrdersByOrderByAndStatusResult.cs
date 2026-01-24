using MilkTea.Shared.Domain.Services;
using MilkTea.Application.DTOs.Orders;

namespace MilkTea.Application.Results.Orders
{
    public class GetOrdersByOrderByAndStatusResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public List<OrderDto> Orders { get; set; } = new();
    }
}

