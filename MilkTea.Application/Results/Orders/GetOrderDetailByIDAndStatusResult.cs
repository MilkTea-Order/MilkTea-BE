using MilkTea.Domain.Entities.Orders;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Results.Orders
{
    public class GetOrderDetailByIDAndStatusResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public Order order = new();
    }
}
