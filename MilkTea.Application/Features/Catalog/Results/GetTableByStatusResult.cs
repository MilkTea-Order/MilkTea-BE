using MilkTea.Application.Models.Catalog;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Catalog.Results
{
    public class GetTableByStatusResult
    {
        public List<TableDto> Tables { get; set; } = new();
        public StringListEntry ResultData { get; set; } = new();
    }
}

