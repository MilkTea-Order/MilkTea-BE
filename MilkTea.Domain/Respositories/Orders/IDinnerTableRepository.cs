using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Domain.Respositories.Orders
{
    public interface IDinnerTableRepository
    {
        Task<DinnerTable?> GetTableByIdAsync(int tableId);
        Task<bool> IsTableAvailableAsync(int tableId);
    }
}