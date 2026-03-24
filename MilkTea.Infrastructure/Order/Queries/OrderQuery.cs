using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Orders.Abstractions;
using MilkTea.Application.Features.Orders.Models.Dtos;
using MilkTea.Application.Ports.Time;
using MilkTea.Domain.Orders.Enums;
using MilkTea.Infrastructure.Persistence;
using Shared.Extensions;

namespace MilkTea.Infrastructure.Order.Queries
{
    public class OrderQuery(AppDbContext context, ITimeZoneServicePort timeZoneServicePort) : IOrderQuery
    {
        private readonly AppDbContext _vContext = context;
        private readonly ITimeZoneServicePort _vTimeZoneServicePort = timeZoneServicePort;

        public async Task<bool> IsTableAvailable(int tableId, CancellationToken cancellationToken = default)
        {
            var hasUnpaidOrder = await _vContext.Orders.AsNoTracking()
                                                        .AnyAsync(x => x.DinnerTableId == tableId &&
                                                                    x.Status == OrderStatus.Unpaid,
                                                        cancellationToken);
            return !hasUnpaidOrder;
        }

        public async Task<List<OrderDto>> GetOrdersAsync(int orderBy, OrderStatus orderStatus, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default)
        {
            var baseQuery = _vContext.Orders.AsNoTracking()
                                        .Where(o => o.OrderBy == orderBy && o.Status == orderStatus);


            if (orderStatus == OrderStatus.Unpaid)
            {
                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.CreatedDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.CreatedDate <= toDate.Value);
            }
            else if (orderStatus == OrderStatus.NotCollected)
            {
                baseQuery = baseQuery.Where(x => x.PaymentedDate != null);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.PaymentedDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.PaymentedDate <= toDate.Value);
            }
            else if (orderStatus == OrderStatus.Paid)
            {
                baseQuery = baseQuery.Where(x => x.ActionDate != null);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.ActionDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.ActionDate <= toDate.Value);
            }
            else if (orderStatus == OrderStatus.Cancelled)
            {
                baseQuery = baseQuery.Where(x => x.CancelledDate != null);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.CancelledDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.CancelledDate <= toDate.Value);
            }


            //if (fromDate.HasValue)
            //    baseQuery = baseQuery.Where(x => x.CreatedDate >= fromDate.Value);

            //if (toDate.HasValue)
            //    baseQuery = baseQuery.Where(x => x.CreatedDate <= toDate.Value);

            var resultQuery = baseQuery.Select(o => new OrderDto
            {
                OrderId = o.Id,
                DinnerTableId = o.DinnerTableId,
                OrderDate = o.OrderDate,
                OrderBy = o.OrderBy,
                CreatedDate = o.CreatedDate,
                CreatedBy = o.CreatedBy,
                PaymentDate = o.PaymentedDate,
                ActionDate = o.ActionDate,
                CancellDate = o.CancelledDate,
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

            if (orderStatus == OrderStatus.Unpaid)
                resultQuery = resultQuery.OrderBy(x => x.DinnerTableId);

            else if (orderStatus == OrderStatus.NotCollected)
                resultQuery = resultQuery.OrderByDescending(x => x.PaymentDate);

            else if (orderStatus == OrderStatus.Paid)
                resultQuery = resultQuery.OrderByDescending(x => x.ActionDate);

            else if (orderStatus == OrderStatus.Cancelled)
                resultQuery = resultQuery.OrderByDescending(x => x.CancellDate);

            return await resultQuery.ToListAsync(cancellationToken);
        }

        public async Task<ReportOrderDto> GetOrderReportAsync(int? orderBy,
                                                                    OrderStatus orderStatus,
                                                                    DateTime? fromDate,
                                                                    DateTime? toDate,
                                                                    string? paymentMethod,
                                                                    CancellationToken cancellationToken = default)
        {
            var tz = _vTimeZoneServicePort.GetTimeZone();

            var baseQuery = _vContext.Orders.AsNoTracking()
                                             .Where(x => x.Status == orderStatus);

            if (orderStatus == OrderStatus.Unpaid)
            {
                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.CreatedDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.CreatedDate <= toDate.Value);
            }
            else if (orderStatus == OrderStatus.NotCollected)
            {
                baseQuery = baseQuery.Where(x => x.PaymentedDate != null);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.PaymentedDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.PaymentedDate <= toDate.Value);
            }
            else if (orderStatus == OrderStatus.Paid)
            {
                baseQuery = baseQuery.Where(x => x.ActionDate != null);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.ActionDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.ActionDate <= toDate.Value);
            }
            else if (orderStatus == OrderStatus.Cancelled)
            {
                baseQuery = baseQuery.Where(x => x.CancelledDate != null);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.CancelledDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.CancelledDate <= toDate.Value);
            }

            if (orderBy.HasValue)
            {
                baseQuery = baseQuery.Where(x => x.OrderBy == orderBy.Value);
            }

            var orderQuery = baseQuery;

            if (!string.IsNullOrWhiteSpace(paymentMethod))
            {
                orderQuery = orderQuery.Where(x => x.PaymentedType == paymentMethod);
            }

            var ordersRaw = await orderQuery.Select(x => new
            {
                Data = new OrderDto
                {
                    OrderId = x.Id,
                    DinnerTableId = x.DinnerTableId,
                    OrderDate = x.OrderDate,
                    OrderBy = x.OrderBy,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    PaymentDate = x.PaymentedDate,
                    TotalAmount = x.TotalAmount ?? 0,
                    Note = x.Note,
                    Status = new OrderStatusDto
                    {
                        Id = (int)x.Status,
                        Name = x.Status.GetDescription()
                    }
                },

                FilterDate = orderStatus == OrderStatus.Cancelled ? x.CancelledDate :
                                            orderStatus == OrderStatus.Paid ? x.ActionDate :
                                            orderStatus == OrderStatus.NotCollected ? x.PaymentedDate :
                                            x.CreatedDate
            }).OrderByDescending(x => x.FilterDate)
                .ToListAsync(cancellationToken);

            var dateGroups = ordersRaw.GroupBy(x =>
            {
                var time = x.FilterDate!.Value;
                return TimeZoneInfo.ConvertTimeFromUtc(time, tz).Date;
            })
            .OrderByDescending(g => g.Key)
            .Select(g => new OrderDateGroupDto
            {
                Date = DateOnly.FromDateTime(g.Key),
                TotalAmount = g.Sum(x => x.Data.TotalAmount),
                Orders = g
                    .OrderByDescending(x => x.FilterDate)
                    .Select(x => x.Data)
                    .ToList()
            })
            .ToList();

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
                DateGroup = dateGroups,
                Statics = resultStatic
            };
        }

    }
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

//public async Task<ReportOrderDto> GetOrderReportAsync(int? orderBy, OrderStatus OrderStatus,
//                                                        DateTime? FromDate, DateTime? ToDate,
//                                                        string? PaymentMethod, CancellationToken cancellationToken = default)
//{

//    var tz = _vTimeZoneServicePort.GetTimeZone();

//    var baseQuery = _vContext.Orders.AsNoTracking().Where(x => x.Status == OrderStatus);

//    if (OrderStatus == Domain.Orders.Enums.OrderStatus.Paid)
//        baseQuery = baseQuery.Where(x => x.PaymentedDate.HasValue);


//    if (FromDate.HasValue)
//        baseQuery = baseQuery.Where(x => x.CreatedDate >= FromDate.Value);

//    if (ToDate.HasValue)
//        baseQuery = baseQuery.Where(x => x.CreatedDate <= ToDate.Value);


//    if (orderBy.HasValue)
//        baseQuery = baseQuery.Where(x => x.OrderBy == orderBy);

//    var orderQuery = baseQuery;

//    if (!string.IsNullOrWhiteSpace(PaymentMethod))
//        orderQuery = orderQuery.Where(x => x.PaymentedType == PaymentMethod);

//    var orders = await orderQuery
//        .Select(x => new OrderDto
//        {
//            OrderId = x.Id,
//            DinnerTableId = x.DinnerTableId,
//            OrderDate = x.OrderDate,
//            OrderBy = x.OrderBy,
//            CreatedBy = x.CreatedBy,
//            CreatedDate = x.CreatedDate,
//            PaymentDate = x.PaymentedDate,
//            PaymentAmount = x.PaymentedTotal,
//            TotalAmount = x.TotalAmount ?? 0,
//            Status = new OrderStatusDto
//            {
//                Id = (int)x.Status,
//                Name = x.Status.GetDescription()
//            },
//            Note = x.Note
//        })
//        .OrderByDescending(x => x.PaymentDate)
//        .ToListAsync(cancellationToken);

//    var dateGroups = orders.GroupBy(x =>
//                                   {
//                                       var time = x.PaymentDate.HasValue ? x.PaymentDate.Value
//                                                                            : x.CreatedDate!.Value;
//                                       return TimeZoneInfo.ConvertTimeFromUtc(time, tz).Date;
//                                   })
//                                   .OrderByDescending(g => g.Key)
//                                   .Select(g => new OrderDateGroupDto
//                                   {
//                                       Date = DateOnly.FromDateTime(g.Key),

//                                       TotalAmount = g.Sum(x => x.TotalAmount),

//                                       Orders = g
//                                           .OrderByDescending(x => x.PaymentDate)
//                                           .ToList()
//                                   })
//                                   .ToList();

//    var staticsData = await baseQuery
//        .GroupBy(x => x.PaymentedType)
//        .Select(g => new
//        {
//            PaymentMethod = g.Key,
//            Total = g.Sum(x => x.PaymentedTotal)
//        })
//        .ToListAsync(cancellationToken);

//    var resultStatic = new StaticDto();

//    foreach (var item in staticsData)
//    {
//        switch (item.PaymentMethod)
//        {
//            case Domain.Orders.Enums.PaymentMethod.CASH:
//                resultStatic.TotalAmountCash = item.Total ?? 0;
//                break;
//            case Domain.Orders.Enums.PaymentMethod.BANK:
//                resultStatic.TotalAmountBank = item.Total ?? 0;
//                break;
//            case Domain.Orders.Enums.PaymentMethod.GRAB:
//                resultStatic.TotalAmountGrab = item.Total ?? 0;
//                break;
//            case Domain.Orders.Enums.PaymentMethod.SHOPEE:
//                resultStatic.TotalAmountShopee = item.Total ?? 0;
//                break;
//        }
//    }

//    resultStatic.TotalAmount =
//        resultStatic.TotalAmountCash +
//        resultStatic.TotalAmountBank +
//        resultStatic.TotalAmountGrab +
//        resultStatic.TotalAmountShopee;

//    return new ReportOrderDto
//    {
//        DateGroup = dateGroups,
//        Statics = resultStatic
//    };
//}