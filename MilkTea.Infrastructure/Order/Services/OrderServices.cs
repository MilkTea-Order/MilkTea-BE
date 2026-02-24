using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Orders.Abstractions.Services;
using MilkTea.Domain.Orders.Enums;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Order.Services
{
    public class OrderServices(AppDbContext context) : IOrderServices
    {
        private readonly AppDbContext _vContext = context;

        public async Task<bool> IsTableAvailable(int tableId, CancellationToken cancellationToken = default)
        {
            var hasUnpaidOrder = await _vContext.Orders
                .AsNoTracking()
                .AnyAsync(x => x.DinnerTableId == tableId
                            && x.Status == OrderStatus.Unpaid,
                          cancellationToken);

            return !hasUnpaidOrder;
        }

    }
}
