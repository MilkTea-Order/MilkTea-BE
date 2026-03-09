using MilkTea.Application.Features.Orders.Models.Dtos;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Models.Results
{
    public class GetOrdersByOrderByAndStatusResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public List<OrderDto> Orders { get; set; } = new();
    }
}

