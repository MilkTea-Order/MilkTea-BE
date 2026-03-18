using MilkTea.Application.Features.Catalog.Models.Dtos.Menu;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Catalog.Models.Results
{
    public class GetMenuItemsOfGroupResult
    {
        public List<MenuDto> Menus { get; set; } = new();
        public StringListEntry ResultData { get; set; } = new();
    }
}

