using MilkTea.Application.Models.Catalog;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Catalog.Results
{
    public class GetMenuItemsOfGroupResult
    {
        public List<MenuItemDto> Menus { get; set; } = new();
        public StringListEntry ResultData { get; set; } = new();
    }
}

