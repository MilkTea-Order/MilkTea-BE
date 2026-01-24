using MilkTea.Shared.Domain.Services;
using MilkTea.Application.DTOs.Orders;

namespace MilkTea.Application.Results.Orders
{
    public class GetOrderDetailByIDAndStatusResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public OrderDetailDto? Order { get; set; }
    }
}
