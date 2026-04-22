using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Orders.Abstractions;
using MilkTea.Application.Features.Orders.Models.Dtos;
using MilkTea.Application.Ports.Time;
using MilkTea.Domain.Orders.Enums;
using MilkTea.Infrastructure.Persistence;
using Shared.Extensions;

namespace MilkTea.Infrastructure.Features.Order.Queries
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

        public async Task<List<OrderDto>> GetOrdersAsync(int actionBy, OrderStatus orderStatus, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default)
        {
            fromDate = fromDate?.Date;
            toDate = toDate?.Date.AddDays(1);

            var baseQuery = _vContext.Orders.AsNoTracking()
                                        .Where(o => o.Status == orderStatus);

            if (orderStatus == OrderStatus.Unpaid)
            {
                baseQuery = baseQuery.Where(x => x.CreatedBy == actionBy);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.CreatedDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.CreatedDate < toDate.Value);
            }
            else if (orderStatus == OrderStatus.NotCollected)
            {
                baseQuery = baseQuery.Where(x => x.PaymentedDate != null && x.PaymentedBy == actionBy);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.PaymentedDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.PaymentedDate < toDate.Value);
            }
            else if (orderStatus == OrderStatus.Paid)
            {
                baseQuery = baseQuery.Where(x => x.ActionDate != null && x.ActionBy == actionBy);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.ActionDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.ActionDate < toDate.Value);
            }
            else if (orderStatus == OrderStatus.Cancelled)
            {
                baseQuery = baseQuery.Where(x => x.CancelledDate != null && x.CancelledBy == actionBy);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.CancelledDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.CancelledDate < toDate.Value);
            }

            var resultQuery = baseQuery.Select(o => new OrderDto
            {
                OrderId = o.Id,
                DinnerTableId = o.DinnerTableId,
                OrderDate = o.OrderDate,
                OrderBy = o.OrderBy,
                CreatedDate = o.CreatedDate,
                CreatedBy = o.CreatedBy,
                ActionBy = o.ActionBy,
                ActionDate = o.ActionDate,
                CancelledBy = o.CancelledBy,
                CancelledDate = o.CancelledDate,
                PaymentBy = o.PaymentedBy,
                PaymentDate = o.PaymentedDate,
                PaymentAmount = o.PaymentedTotal,
                PaymentMethod = o.PaymentedType,
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

            Console.WriteLine(resultQuery.ToQueryString());
            return await resultQuery.ToListAsync(cancellationToken);
        }

        public async Task<ReportOrderDto> GetOrderReportAsync(int actionBy,
                                                                OrderStatus orderStatus,
                                                                DateTime? fromDate,
                                                                DateTime? toDate,
                                                                string? paymentMethod,
                                                                CancellationToken cancellationToken = default)
        {
            if (orderStatus != OrderStatus.Paid && orderStatus != OrderStatus.NotCollected)
            {
                return new ReportOrderDto
                {
                    DateGroup = new List<OrderDateGroupDto>(),
                    Statics = new StaticDto()
                };
            }

            fromDate = fromDate?.Date;
            toDate = toDate?.Date.AddDays(1); // exclusive upper bound

            var baseQuery = _vContext.Orders.AsNoTracking()
                                                .Where(x => x.Status == orderStatus);

            if (orderStatus == OrderStatus.NotCollected)
            {
                baseQuery = baseQuery.Where(x => x.PaymentedDate != null && x.PaymentedBy == actionBy);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.PaymentedDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.PaymentedDate < toDate.Value);
            }
            else // Paid
            {
                baseQuery = baseQuery.Where(x => x.ActionDate != null && x.ActionBy == actionBy);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.ActionDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.ActionDate < toDate.Value);
            }

            var ordersRaw = await baseQuery
                .Where(x => string.IsNullOrWhiteSpace(paymentMethod) || x.PaymentedType == paymentMethod)
                .Select(x => new
                {
                    Data = new OrderDto
                    {
                        OrderId = x.Id,
                        DinnerTableId = x.DinnerTableId,
                        OrderDate = x.OrderDate,
                        OrderBy = x.OrderBy,
                        CreatedDate = x.CreatedDate,
                        CreatedBy = x.CreatedBy,
                        ActionBy = x.ActionBy,
                        ActionDate = x.ActionDate,
                        CancelledBy = x.CancelledBy,
                        CancelledDate = x.CancelledDate,
                        PaymentBy = x.PaymentedBy,
                        PaymentDate = x.PaymentedDate,
                        PaymentAmount = x.PaymentedTotal,
                        PaymentMethod = x.PaymentedType,
                        TotalAmount = x.TotalAmount ?? 0,
                        Note = x.Note,
                        Status = new OrderStatusDto
                        {
                            Id = (int)x.Status,
                            Name = x.Status.GetDescription()
                        }
                    },

                    FilterDate = orderStatus == OrderStatus.Paid
                        ? x.ActionDate
                        : x.PaymentedDate
                })
                .OrderByDescending(x => x.FilterDate)
                .ToListAsync(cancellationToken);

            var dateGroups = ordersRaw
                .GroupBy(x => DateOnly.FromDateTime(x.FilterDate!.Value.Date))
                .OrderByDescending(g => g.Key)
                .Select(g => new OrderDateGroupDto
                {
                    Date = g.Key,
                    TotalAmount = g.Sum(x => x.Data.TotalAmount),
                    Orders = g
                        .OrderByDescending(x => x.FilterDate)
                        .Select(x => x.Data)
                        .ToList()
                })
                .ToList();

            var staticQuery = await baseQuery
                .Where(x => x.PaymentedType != null)
                .GroupBy(x => x.PaymentedType!)
                .Select(g => new
                {
                    PaymentMethod = g.Key,
                    Total = g.Sum(x => x.PaymentedTotal ?? 0m)
                })
                .ToListAsync(cancellationToken);

            var resultStatic = new StaticDto();

            foreach (var item in staticQuery)
            {
                switch (item.PaymentMethod)
                {
                    case PaymentMethod.CASH:
                        resultStatic.TotalAmountCash = item.Total;
                        break;
                    case PaymentMethod.BANK:
                        resultStatic.TotalAmountBank = item.Total;
                        break;
                    case PaymentMethod.GRAB:
                        resultStatic.TotalAmountGrab = item.Total;
                        break;
                    case PaymentMethod.SHOPEE:
                        resultStatic.TotalAmountShopee = item.Total;
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
