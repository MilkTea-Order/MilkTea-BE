using MilkTea.Application.Features.Inventory.Models.Dtos;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Inventory.Models.Results
{
    public class GetInventorySummaryResult
    {
        public StringListEntry ResultData { get; set; } = new();

        public List<MaterialInventoryDto> Materials { get; set; } = new();
    }
}
