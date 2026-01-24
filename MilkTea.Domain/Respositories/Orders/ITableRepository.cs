using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Domain.Respositories.Orders
{
    public interface ITableRepository
    {
        Task<List<DinnerTable>> GetTablesByStatusAsync(int? statusId);

        Task<List<DinnerTable>> GetTableEmpty();
    }
}
