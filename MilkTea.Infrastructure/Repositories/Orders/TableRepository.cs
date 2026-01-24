using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Constants.Enums;
using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Orders
{
    public class TableRepository(AppDbContext context) : ITableRepository
    {
        private readonly AppDbContext _vContext = context;
        public async Task<List<DinnerTable>> GetTablesByStatusAsync(int? statusId)
        {
            var query = _vContext.DinnerTable
                .AsNoTracking()
                .Include(x => x.StatusOfDinnerTable)
                .AsQueryable();

            if (statusId.HasValue)
            {
                query = query.Where(x => x.StatusOfDinnerTableID == statusId.Value);
            }

            return await query
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<DinnerTable>> GetTableEmpty()
        {
            var query =
                from dt in _vContext.DinnerTable
                join o in _vContext.Orders
                    on dt.ID equals o.DinnerTableID
                where dt.StatusOfDinnerTableID == (int)TableStatus.Valiable
                   && o.StatusOfOrderID == (int)OrderStatus.Unpaid
                select dt;

            var result = await query
                .Include(x => x.StatusOfDinnerTable)
                .Distinct()
                .OrderBy(x => x.Name)
                .ToListAsync();
            return result;
        }
    }
}
