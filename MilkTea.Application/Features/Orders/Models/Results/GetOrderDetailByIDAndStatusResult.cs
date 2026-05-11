using MilkTea.Application.Features.Orders.Models.Dtos;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Models.Results
{
    public class GetOrderDetailByIDAndStatusResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public OrderDto? Order { get; set; }
    }
}

