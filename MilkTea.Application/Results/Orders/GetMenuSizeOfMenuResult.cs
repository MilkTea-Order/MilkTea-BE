using MilkTea.Shared.Domain.Services;
using MilkTea.Application.DTOs.Orders;

namespace MilkTea.Application.Results.Orders
{
    public class GetMenuSizeOfMenuResult
    {
        public List<MenuSizePriceDto> MenuSize { get; set; } = new();
        public StringListEntry ResultData { get; set; } = new();
    }
}
