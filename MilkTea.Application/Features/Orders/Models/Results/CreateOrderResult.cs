using MilkTea.Shared.Domain.Services;
using MilkTea.Application.Features.Orders.Models.Dtos;

namespace MilkTea.Application.Features.Orders.Models.Results
{
    public class CreateOrderResult
    {
        public OrderDto? Order;
        public StringListEntry ResultData { get; set; } = new();
    }
}

