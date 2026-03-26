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
        public async Task<List<OrderDto>> GetOrdersAsync(int actionBy, OrderStatus orderStatus, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default)
        {
            var baseQuery = _vContext.Orders.AsNoTracking()
                                        .Where(o => o.Status == orderStatus);

            if (orderStatus == OrderStatus.Unpaid)
            {
                baseQuery = baseQuery.Where(x => x.CreatedBy == actionBy);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.CreatedDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.CreatedDate <= toDate.Value);
            }
            else if (orderStatus == OrderStatus.NotCollected)
            {
                baseQuery = baseQuery.Where(x => x.PaymentedDate != null && x.PaymentedBy == actionBy);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.PaymentedDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.PaymentedDate <= toDate.Value);
            }
            else if (orderStatus == OrderStatus.Paid)
            {
                baseQuery = baseQuery.Where(x => x.ActionDate != null && x.ActionBy == actionBy);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.ActionDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.ActionDate <= toDate.Value);
            }
            else if (orderStatus == OrderStatus.Cancelled)
            {
                baseQuery = baseQuery.Where(x => x.CancelledDate != null && x.CancelledBy == actionBy);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.CancelledDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.CancelledDate <= toDate.Value);
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

            return await resultQuery.ToListAsync(cancellationToken);
        }
        public async Task<ReportOrderDto> GetOrderReportAsync(int? actionBy, OrderStatus orderStatus, DateTime? fromDate, DateTime? toDate, string? paymentMethod, CancellationToken cancellationToken = default)
        {

            if (orderStatus != OrderStatus.Paid && orderStatus != OrderStatus.NotCollected)
            {
                return new ReportOrderDto
                {
                    DateGroup = new List<OrderDateGroupDto>(),
                    Statics = new StaticDto()
                };
            }

            var tz = _vTimeZoneServicePort.GetTimeZone();
            var baseQuery = _vContext.Orders.AsNoTracking()
                                             .Where(x => x.Status == orderStatus);

            if (orderStatus == OrderStatus.NotCollected)
            {
                baseQuery = baseQuery.Where(x => x.PaymentedDate != null);

                if (actionBy.HasValue)
                    baseQuery = baseQuery.Where(x => x.PaymentedBy == actionBy.Value);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.PaymentedDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.PaymentedDate <= toDate.Value);
            }
            else if (orderStatus == OrderStatus.Paid)
            {
                baseQuery = baseQuery.Where(x => x.ActionDate != null);

                if (actionBy.HasValue)
                    baseQuery = baseQuery.Where(x => x.ActionBy == actionBy.Value);

                if (fromDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.ActionDate >= fromDate.Value);

                if (toDate.HasValue)
                    baseQuery = baseQuery.Where(x => x.ActionDate <= toDate.Value);
            }

            var staticQuery = baseQuery
                .Where(x => x.PaymentedType != null)
                .AsEnumerable()
                .Select(x => new
                {
                    PaymentMethod = x.PaymentedType!,
                    Total = x.PaymentedTotal ?? 0m
                })
                .GroupBy(x => x.PaymentMethod)
                .Select(g => new
                {
                    PaymentMethod = g.Key,
                    Total = g.Sum(x => x.Total)
                })
                .ToList();


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

                FilterDate = orderStatus == OrderStatus.Paid ? x.ActionDate : x.PaymentedDate
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

            var resultStatic = new StaticDto();

            foreach (var item in staticQuery)
            {
                switch (item.PaymentMethod)
                {
                    case Domain.Orders.Enums.PaymentMethod.CASH:
                        resultStatic.TotalAmountCash = item.Total;
                        break;
                    case Domain.Orders.Enums.PaymentMethod.BANK:
                        resultStatic.TotalAmountBank = item.Total;
                        break;
                    case Domain.Orders.Enums.PaymentMethod.GRAB:
                        resultStatic.TotalAmountGrab = item.Total;
                        break;
                    case Domain.Orders.Enums.PaymentMethod.SHOPEE:
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
