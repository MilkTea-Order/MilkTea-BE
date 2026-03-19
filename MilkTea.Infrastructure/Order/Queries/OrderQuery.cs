using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Orders.Abstractions;
using MilkTea.Application.Features.Orders.Models.Dtos;
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

        public async Task<List<OrderDto>> GetOrdersAsync(int orderBy, int? status, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default)
        {
            var query = _vContext.Orders.AsNoTracking()
                                        .Where(o => o.OrderBy == orderBy);

            if (status.HasValue)
                query = query.Where(o => (int)o.Status == status.Value);

            if (fromDate.HasValue)
                query = query.Where(o => o.CreatedDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(o => o.CreatedDate <= toDate.Value);

            var resultQuery = query
                .Select(o => new OrderDto
                {
                    OrderId = o.Id,
                    DinnerTableId = o.DinnerTableId,
                    OrderDate = o.OrderDate,
                    OrderBy = o.OrderBy,
                    CreatedDate = o.CreatedDate,
                    CreatedBy = o.CreatedBy,
                    PaymentDate = o.PaymentedDate,
                    ActionDate = o.ActionDate,
                    Note = o.Note,
                    Status = new OrderStatusDto
                    {
                        Id = (int)o.Status,
                        Name = o.Status.GetDescription()
                    },

                    TotalAmount = _vContext.OrderItems
                        .Where(d => d.OrderId == o.Id
                                 && !(d.CancelledBy != null && d.CancelledDate != null))
                        .Sum(d => d.Quantity * d.MenuItem.Price)
                });

            if (status == (int)OrderStatus.Unpaid)
                resultQuery = resultQuery.OrderBy(x => x.DinnerTableId);

            else if (status == (int)OrderStatus.NotCollected)
                resultQuery = resultQuery.OrderByDescending(x => x.PaymentDate);

            else if (status == (int)OrderStatus.Paid)
                resultQuery = resultQuery.OrderByDescending(x => x.ActionDate);

            else if (status == (int)OrderStatus.Cancelled)
                resultQuery = resultQuery.OrderByDescending(x => x.CreatedDate);

            return await resultQuery.ToListAsync(cancellationToken);
        }

        public async Task<ReportOrderDto> GetOrderReportAsync(int? orderBy, DateTime? FromDate, DateTime? ToDate, string? PaymentMethod, CancellationToken cancellationToken = default)
        {
            var baseQuery = _vContext.Orders.AsNoTracking().Where(x => x.Status == OrderStatus.NotCollected);

            if (orderBy.HasValue)
                baseQuery = baseQuery.Where(x => x.OrderBy == orderBy);

            if (FromDate.HasValue)
                baseQuery = baseQuery.Where(x => x.CreatedDate >= FromDate.Value);

            if (ToDate.HasValue)
                baseQuery = baseQuery.Where(x => x.CreatedDate <= ToDate.Value);

            var orderQuery = baseQuery;
            if (!string.IsNullOrWhiteSpace(PaymentMethod))
                orderQuery = orderQuery.Where(x => x.PaymentedType == PaymentMethod);

            var orders = await orderQuery
                .Select(x => new OrderDto
                {
                    OrderId = x.Id,
                    DinnerTableId = x.DinnerTableId,
                    OrderDate = x.OrderDate,
                    OrderBy = x.OrderBy,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    PaymentDate = x.PaymentedDate,
                    PaymentAmount = x.PaymentedTotal,
                    TotalAmount = x.TotalAmount ?? 0,
                    Status = new OrderStatusDto
                    {
                        Id = (int)x.Status,
                        Name = x.Status.GetDescription()
                    },
                    Note = x.Note
                })
                .OrderByDescending(x => x.PaymentDate)
                .ToListAsync(cancellationToken);

            var staticsData = await baseQuery
                .GroupBy(x => x.PaymentedType)
                .Select(g => new
                {
                    PaymentMethod = g.Key,
                    Total = g.Sum(x => x.PaymentedTotal)
                })
                .ToListAsync(cancellationToken);

            var resultStatic = new StaticDto();

            foreach (var item in staticsData)
            {
                switch (item.PaymentMethod)
                {
                    case Domain.Orders.Enums.PaymentMethod.CASH:
                        resultStatic.TotalAmountCash = item.Total ?? 0;
                        break;
                    case Domain.Orders.Enums.PaymentMethod.BANK:
                        resultStatic.TotalAmountBank = item.Total ?? 0;
                        break;
                    case Domain.Orders.Enums.PaymentMethod.GRAB:
                        resultStatic.TotalAmountGrab = item.Total ?? 0;
                        break;
                    case Domain.Orders.Enums.PaymentMethod.SHOPEE:
                        resultStatic.TotalAmountShopee = item.Total ?? 0;
                        break;
                }
            }

            resultStatic.TotalAmount =
                resultStatic.TotalAmountCash +
                resultStatic.TotalAmountBank +
                resultStatic.TotalAmountGrab +
                resultStatic.TotalAmountShopee;

            return new ReportOrderDto
            {
                Orders = orders,
                Statics = resultStatic
            };
        }


        //public async Task<List<OrderDto>> GetOrdersAsync(int orderBy, int? status, int? dayAgo, CancellationToken cancellationToken = default)
        //{

        //    DateTime? startDate = null;
        //    DateTime? endDate = null;

        //    var tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        //    var todayLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz).Date;
        //    var utc = TimeZoneInfo.ConvertTimeToUtc(todayLocal, tz);

        //    if (dayAgo.HasValue)
        //    {
        //        if (dayAgo.Value == 0)
        //        {
        //            startDate = utc;
        //            endDate = utc.AddDays(1);
        //        }
        //        else
        //        {
        //            startDate = utc.AddDays(-dayAgo.Value);
        //            endDate = utc;
        //        }
        //    }
        //    var query = from o in _vContext.Orders.AsNoTracking()
        //                join d in _vContext.OrderItems.AsNoTracking()
        //                    on o.Id equals d.OrderId
        //                where o.OrderBy == orderBy
        //                    && (!status.HasValue || (int)o.Status == status.Value)
        //                    && (!dayAgo.HasValue || (o.CreatedDate >= startDate && o.CreatedDate < endDate))
        //                group d by new
        //                {
        //                    o.Id,
        //                    o.DinnerTableId,
        //                    o.OrderDate,
        //                    o.OrderBy,
        //                    o.CreatedDate,
        //                    o.CreatedBy,
        //                    o.Status,
        //                    o.Note
        //                } into g
        //                select new OrderDto
        //                {
        //                    OrderId = g.Key.Id,
        //                    DinnerTableId = g.Key.DinnerTableId,
        //                    OrderDate = g.Key.OrderDate,
        //                    OrderBy = g.Key.OrderBy,
        //                    CreatedDate = g.Key.CreatedDate,
        //                    CreatedBy = g.Key.CreatedBy,
        //                    Status = new OrderStatusDto
        //                    {
        //                        Id = (int)g.Key.Status,
        //                        Name = g.Key.Status.GetDescription()
        //                    },
        //                    Note = g.Key.Note,
        //                    TotalAmount = g.Sum(x => (x.CancelledBy != null && x.CancelledDate != null ? 0 : x.Quantity * x.MenuItem.Price))
        //                };
        //    //Console.WriteLine(query.ToQueryString());
        //    return await query.OrderByDescending(o => o.CreatedDate).ToListAsync(cancellationToken);

        //}
    }
}
