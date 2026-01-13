using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Results.Orders
{
    public class GetMenuItemsOfGroupResult
    {
        public List<Dictionary<string, object?>> Menus { get; set; } = default!;
        public StringListEntry ResultData { get; set; } = new();
    }
}
