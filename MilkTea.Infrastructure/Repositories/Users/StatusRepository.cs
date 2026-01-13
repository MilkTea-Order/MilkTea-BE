using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Entities.Users;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Infrastructure.Persistence;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Infrastructure.Repositories.Users
{
    public class StatusRepository(AppDbContext context) : IStatusRepository
    {
        private readonly AppDbContext _vContext = context;
        public async Task<bool> ExistsDinnerTableStatusAsync(int statusId)
        {
            return await _vContext.StatusOfDinnerTable.AsNoTracking().AnyAsync(x => x.ID == statusId);
        }
        public async Task<bool> ExistsStatusAsync(int statusId)
        {
            return await _vContext.Status.AsNoTracking().AnyAsync(x => x.ID == statusId);
        }
        public async Task<Status?> GetActive()
        {
            return await GetByName(StatusName.NAME_ACTIVE);
        }

        public async Task<Status?> GetById(int id)
        {
            return await _vContext.Status.FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<IReadOnlyList<StatusOfDinnerTable>> GetDinnerTableStatusesAsync()
        {
            return await _vContext.StatusOfDinnerTable.AsNoTracking().OrderBy(x => x.ID).ToListAsync();
        }

        private async Task<Status?> GetByName(string name)
        {
            return await _vContext.Status.FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
