using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Constants.Enums;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Infrastructure.Persistence;
using MilkTea.Shared.Extensions;

namespace MilkTea.Infrastructure.Repositories.Orders
{
    public class TableRepository(AppDbContext context) : ITableRepository
    {
        private readonly AppDbContext _vContext = context;
        public async Task<List<Dictionary<string, object?>>> GetTablesByStatusAsync(int? statusID)
        {
            var query =
                from t in _vContext.DinnerTable
                join st in _vContext.StatusOfDinnerTable
                    on t.StatusOfDinnerTableID equals st.ID into sts
                from st in sts.DefaultIfEmpty()
                select new
                {
                    tableID = t.ID,
                    tableCode = t.Code,
                    tableName = t.Name,
                    numberOfSeat = t.NumberOfSeats,
                    tableNote = t.Note,
                    statusID = t.StatusOfDinnerTableID,
                    statusName = st != null ? st.Name : "Không rõ"
                };

            if (statusID.HasValue)
            {
                query = query.Where(x => x.statusID == statusID.Value);
            }
            return (await query.AsNoTracking().ToListAsync()).ToDictList();
        }

        public async Task<List<Domain.Entities.Orders.DinnerTable>> GetTableEmpty()
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
