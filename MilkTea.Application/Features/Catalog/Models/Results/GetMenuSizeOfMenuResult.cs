using MilkTea.Application.Features.Catalog.Models.Dtos;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Catalog.Models.Results
{
    public class GetMenuSizeOfMenuResult
    {
        public List<SizeAndPriceDto> MenuSize { get; set; } = new();
        public StringListEntry ResultData { get; set; } = new();
    }
}

