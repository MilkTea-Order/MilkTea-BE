using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Results.Orders
{
    public class GetGroupMenuResult
    {
        public List<Dictionary<string, object?>> GroupMenu { get; set; } = default!;
        public StringListEntry ResultData { get; set; } = new();
    }
}
