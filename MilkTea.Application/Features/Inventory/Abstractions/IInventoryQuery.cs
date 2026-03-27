using MilkTea.Application.Features.Inventory.Models.Dtos;

namespace MilkTea.Application.Features.Inventory.Abstractions
{
    public interface IInventoryQuery
    {
        Task<List<InventoryStockDto>> GetInventoryReportAsync(List<int>? materialIds = null);
    }
}
