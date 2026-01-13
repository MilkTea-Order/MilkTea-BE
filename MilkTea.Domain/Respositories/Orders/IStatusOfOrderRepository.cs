using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Domain.Respositories.Orders
{
    public interface IStatusOfOrderRepository
    {
        Task<StatusOfOrder?> GetPendingStatusAsync();
        Task<bool> ExistsAsync(int statusId);
        Task<StatusOfOrder?> GetCancelledStatusAsync();
    }
}