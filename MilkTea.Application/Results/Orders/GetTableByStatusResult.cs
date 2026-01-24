using MilkTea.Application.DTOs.Orders;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Results.Orders
{
    public class GetTableByStatusResult
    {
        public List<DinnerTableDto> Tables { get; set; } = new();
        public StringListEntry ResultData { get; set; } = new();
    }
}
