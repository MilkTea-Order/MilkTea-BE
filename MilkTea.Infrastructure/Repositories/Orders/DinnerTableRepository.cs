using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Orders
{
    public class DinnerTableRepository(AppDbContext context) : IDinnerTableRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<DinnerTable?> GetTableByIdAsync(int tableId)
        {
            return await _context.DinnerTable
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.ID == tableId);
        }

        public async Task<bool> IsTableAvailableAsync(int tableId)
        {
            var table = await GetTableByIdAsync(tableId);
            return table != null;
        }
    }
}