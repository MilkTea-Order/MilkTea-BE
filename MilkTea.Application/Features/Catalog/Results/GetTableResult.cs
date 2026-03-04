
using MilkTea.Application.Features.Catalog.Dtos;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Catalog.Results
{
    public sealed class GetTableResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public List<TableDto> Tables { get; set; } = new();
    }
}

