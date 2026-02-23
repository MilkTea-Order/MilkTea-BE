using MilkTea.Application.Features.Catalog.Dtos;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Catalog.Results
{
    public class GetMenuItemsOfGroupResult
    {
        public List<MenuDto> Menus { get; set; } = new();
        public StringListEntry ResultData { get; set; } = new();
    }
}

