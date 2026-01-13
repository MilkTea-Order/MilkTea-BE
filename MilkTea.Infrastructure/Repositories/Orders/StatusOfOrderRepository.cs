using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Infrastructure.Persistence;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Infrastructure.Repositories.Orders
{
    public class StatusOfOrderRepository(AppDbContext context) : IStatusOfOrderRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<StatusOfOrder?> GetPendingStatusAsync()
        {
            return await _context.StatusOfOrder.AsNoTracking().FirstOrDefaultAsync(s => s.Name == StatusName.NAME_NOT_PAYMENT);
        }

        public async Task<StatusOfOrder?> GetCancelledStatusAsync()
        {
            return await _context.StatusOfOrder.AsNoTracking()
                .FirstOrDefaultAsync(s => s.Name == StatusName.NAME_CANCELLED);
        }

        public async Task<bool> ExistsAsync(int statusId)
        {
            return await _context.StatusOfOrder
                .AsNoTracking()
                .AnyAsync(s => s.ID == statusId);
        }
    }
}