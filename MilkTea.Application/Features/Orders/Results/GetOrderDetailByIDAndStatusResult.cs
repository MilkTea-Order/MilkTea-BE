using MilkTea.Application.DTOs.Orders;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Results
{
    public class GetOrderDetailByIDAndStatusResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public OrderDetailDto? Order { get; set; }
    }
}

