using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Orders.Abstractions;
using MilkTea.Application.Features.Orders.Dtos;
using MilkTea.Domain.Orders.Enums;
using MilkTea.Infrastructure.Persistence;
using Shared.Extensions;

namespace MilkTea.Infrastructure.Order.Queries
{
    public class OrderQuery(AppDbContext context) : IOrderQuery
    {
        private readonly AppDbContext _vContext = context;

        public async Task<bool> IsTableAvailable(int tableId, CancellationToken cancellationToken = default)
        {
            var hasUnpaidOrder = await _vContext.Orders.AsNoTracking()
                                                        .AnyAsync(x => x.DinnerTableId == tableId &&
                                                                    x.Status == OrderStatus.Unpaid,
                                                        cancellationToken);
            return !hasUnpaidOrder;
        }

        public async Task<List<OrderDto>> GetOrdersAsync(int orderBy, int? status, int? dayAgo, CancellationToken cancellationToken = default)
        {

            DateTime? startDate = null;
            DateTime? endDate = null;

            var tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var todayLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz).Date;
            var utc = TimeZoneInfo.ConvertTimeToUtc(todayLocal, tz);

            if (dayAgo.HasValue)
            {
                if (dayAgo.Value == 0)
                {
                    startDate = utc;
                    endDate = utc.AddDays(1);
                }
                else
                {
                    startDate = utc.AddDays(-dayAgo.Value);
                    endDate = utc;
                }
            }
            var query = from o in _vContext.Orders.AsNoTracking()
                        join d in _vContext.OrderItems.AsNoTracking()
                            on o.Id equals d.OrderId
                        where o.OrderBy == orderBy
                            && (!status.HasValue || (int)o.Status == status.Value)
                            && (!dayAgo.HasValue || (o.CreatedDate >= startDate && o.CreatedDate < endDate))
                        group d by new
                        {
                            o.Id,
                            o.DinnerTableId,
                            o.OrderDate,
                            o.OrderBy,
                            o.CreatedDate,
                            o.CreatedBy,
                            o.Status,
                            o.Note
                        } into g
                        select new OrderDto
                        {
                            OrderId = g.Key.Id,
                            DinnerTableId = g.Key.DinnerTableId,
                            OrderDate = g.Key.OrderDate,
                            OrderBy = g.Key.OrderBy,
                            CreatedDate = g.Key.CreatedDate,
                            CreatedBy = g.Key.CreatedBy,
                            Status = new OrderStatusDto
                            {
                                Id = (int)g.Key.Status,
                                Name = g.Key.Status.GetDescription()
                            },
                            Note = g.Key.Note,
                            TotalAmount = g.Sum(x => (x.CancelledBy != null && x.CancelledDate != null ? 0 : x.Quantity * x.MenuItem.Price))
                        };
            //Console.WriteLine(query.ToQueryString());
            return await query.OrderByDescending(o => o.CreatedDate).ToListAsync(cancellationToken);

        }
    }
}
