using Microsoft.EntityFrameworkCore;
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
    }
}
