using MilkTea.Application.Models.Orders;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Models.Results
{
    public class CreateOrderResult
    {
        public Order? Order;
        public StringListEntry ResultData { get; set; } = new();
    }
}

