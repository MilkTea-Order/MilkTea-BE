using MilkTea.Application.Models.Catalog;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Catalog.Results
{
    public sealed class GetTableEmptyResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public List<TableDto> Tables { get; set; } = new();
    }
}

