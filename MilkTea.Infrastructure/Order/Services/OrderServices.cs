using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Orders.Abstractions.Services;
using MilkTea.Domain.Orders.Enums;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Order.Services
{
    public class OrderServices(AppDbContext context) : IOrderServices
    {
        private readonly AppDbContext _vContext = context;

        public async Task<List<int>> GetTablesByAvailability(List<int> tableIds,
                                                                bool isAvailable,
                                                                CancellationToken cancellationToken = default)
        {
            var busyTables = await _vContext.Orders.AsNoTracking()
                                                    .Where(o => tableIds.Contains(o.DinnerTableId)
                                                             && o.Status == OrderStatus.Unpaid)
                                                    .Select(o => o.DinnerTableId)
                                                    .Distinct()
                                                    .ToListAsync(cancellationToken);

            return isAvailable
                ? tableIds.Except(busyTables).ToList()
                : tableIds.Intersect(busyTables).ToList();
        }

    }
}
