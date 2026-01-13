using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Results.Orders
{
    public class GetOrdersByOrderByAndStatusResult
    {
        public StringListEntry ResultData { get; set; } = new();
        //public List<Dictionary<string, object?>> orders = new();
        public List<Dictionary<string, object?>> orders = new();
    }
}

