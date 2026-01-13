using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Results.Orders
{
    public class GetMenuSizeOfMenuResult
    {
        public List<Dictionary<string, object?>> MenuSize { get; set; } = default!;
        public StringListEntry ResultData { get; set; } = new();
    }
}
