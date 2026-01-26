using MilkTea.Application.DTOs.Orders;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Results
{
    public class GetOrdersByOrderByAndStatusResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public List<OrderDto> Orders { get; set; } = new();
    }
}

