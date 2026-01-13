using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Entities.Users;

namespace MilkTea.Domain.Respositories.Users
{
    public interface IStatusRepository
    {
        Task<Status?> GetActive();
        Task<Status?> GetById(int id);
        Task<IReadOnlyList<StatusOfDinnerTable>> GetDinnerTableStatusesAsync();
        Task<bool> ExistsDinnerTableStatusAsync(int statusId);
        Task<bool> ExistsStatusAsync(int statusId);
    }
}
