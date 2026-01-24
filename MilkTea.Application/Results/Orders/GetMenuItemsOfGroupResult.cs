using MilkTea.Shared.Domain.Services;
using MilkTea.Application.DTOs.Orders;

namespace MilkTea.Application.Results.Orders
{
    public class GetMenuItemsOfGroupResult
    {
        public List<MenuItemDto> Menus { get; set; } = new();
        public StringListEntry ResultData { get; set; } = new();
    }
}
