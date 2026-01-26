using MilkTea.Application.DTOs.Orders;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.TableManagement.Results
{
    public sealed class GetTableEmptyResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public List<DinnerTableDto> Tables { get; set; } = new();
    }
}

