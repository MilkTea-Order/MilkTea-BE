using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Domain.Respositories.Orders
{
    public interface ITableRepository
    {
        Task<List<Dictionary<string, object?>>> GetTablesByStatusAsync(int? statusID);

        Task<List<DinnerTable>> GetTableEmpty();
    }
}
