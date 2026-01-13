using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Results.Orders
{
    public class GetTableByStatusResult
    {
        public List<Dictionary<string, object?>> Tables { get; set; } = default!;
        public StringListEntry ResultData { get; set; } = new();
    }
}
