using MilkTea.Application.Features.Catalog.Models.Dtos.Menu;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Catalog.Models.Results
{
    public class GetGroupMenuResult
    {
        public List<MenuGroupDto> GroupMenu { get; set; } = new();
        public StringListEntry ResultData { get; set; } = new();
    }
}

