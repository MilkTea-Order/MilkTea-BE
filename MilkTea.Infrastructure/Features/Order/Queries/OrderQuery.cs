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
                DinnerTable = new TableDto
                {
                    Id =  o.DinnerTableId,
                },
                Status = new OrderStatusDto
                {
                    Id = (int)o.Status,
                    Name = o.Status.GetDescription()
                },
                // TotalAmount = _vContext.OrderItems
                //     .Where(d => d.OrderId == o.Id
                //              && !(d.CancelledBy != null && d.CancelledDate != null))
                //     .Sum(d => d.Quantity * d.MenuItem.Price)
                TotalAmount = Math.Round(o.OrderItems
                    .Where(i => i.Status != OrderItemStatus.Cancelled)
                    .Sum(i => i.MenuItem.Price * i.Quantity), 3)
            });

            if (orderStatus == OrderStatus.Unpaid)
                resultQuery = resultQuery.OrderBy(x => x.DinnerTable!.Id);
            else if (orderStatus == OrderStatus.NotCollected)
                resultQuery = resultQuery.OrderByDescending(x => x.PaymentDate);
            else if (orderStatus == OrderStatus.Paid)
                resultQuery = resultQuery.OrderByDescending(x => x.ActionDate);
            else if (orderStatus == OrderStatus.Cancelled)
                resultQuery = resultQuery.OrderByDescending(x => x.CancellDate);

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
                        DinnerTable = new TableDto
                        {
                            Id = x.DinnerTableId,
                        },
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

        public async Task<List<KitchenOrderDto>> GetKitchenOrdersAsync(OrderStatus orderStatus,
                                                                        OrderItemStatus orderDetailStatus,
                                                                        CancellationToken cancellationToken = default)
        {
            var todayStart = DateTime.Now.Date;
            var todayEnd = todayStart.AddDays(1);
            var rows = await _vContext.Orders.AsNoTracking()
                .Where(o => o.Status == orderStatus && o.CreatedDate >= todayStart && o.CreatedDate < todayEnd)
                .Join(
                    _vContext.OrderItems.AsNoTracking()
                    .Where(i => i.Status == orderDetailStatus),
                    o => o.Id,
                    i => i.OrderId,
                    (o, i) => new
                    {
                        OrderId = o.Id,
                        o.DinnerTableId,
                        OrderCreatedDate = o.CreatedDate,
                        OrderCreatedBy = o.CreatedBy,
                        OrderNote = o.Note,
                        OrderStatus = o.Status,
                        OrderItemId = i.Id,
                        ItemQuantity = i.Quantity,
                        OrderItemNote = i.Note,
                        ItemStatus = i.Status,
                        KindOfHotpot1Id = i.MenuItem.KindOfHotpot1Id,
                        KindOfHotpot2Id = i.MenuItem.KindOfHotpot2Id,
                        MenuId = i.MenuItem.MenuId,
                        SizeId = i.MenuItem.SizeId,
                        Price = i.MenuItem.Price,
                        PriceListId = i.MenuItem.PriceListId,
                        ItemCreatedDate = i.CreatedDate,
                        ItemPerformDate = i.PerformDate,
                        ItemCompletedDate = i.CompletedDate,
                        ItemCancelledDate = i.CancelledDate
                    })
                .ToListAsync(cancellationToken);

            return rows.GroupBy(r => new { r.ItemCreatedDate, r.OrderId, r.DinnerTableId })
                .Select(g =>
                {
                    var first = g.First();
                    return new KitchenOrderDto
                    {
                        OrderId = first.OrderId,
                        CreatedDate = first.OrderCreatedDate,
                        CreatedBy = first.OrderCreatedBy,
                        Note = first.OrderNote,
                        Status = new OrderStatusDto
                        {
                            Id = (int)first.OrderStatus,
                            Name = first.OrderStatus.GetDescription()
                        },
                        DinnerTable = new TableDto
                        {
                            Id = first.DinnerTableId
                        },
                        OrderItems = g.Select(r => new KitchenOrderItemDto
                        {
                            Id = r.OrderItemId,
                            OrderId = r.OrderId,
                            Quantity = r.ItemQuantity,
                            Note = r.OrderItemNote,
                            Status = new OrderStatusDto
                            {
                                Id = (int)r.ItemStatus,
                                Name = r.ItemStatus.GetDescription()
                            },
                            Menu = new MenuDto
                            {
                                Id = r.MenuId
                            },
                            Size = new SizeDto
                            {
                                Id = r.SizeId
                            },
                            KindOfHotpot1Id = r.KindOfHotpot1Id,
                            KindOfHotpot2Id = r.KindOfHotpot2Id,
                            CreatedDate = r.ItemCreatedDate,
                            PerformDate = r.ItemPerformDate,
                            CompletedDate = r.ItemCompletedDate,
                            CancelledDate = r.ItemCancelledDate
                        }).ToList()
                    };
                })
                .ToList();
        }

        public async Task<OrderDto?> GetOrderDetailByIdAndStatusAsync(int orderId, bool? isCancelled, CancellationToken cancellationToken = default)
        {
            var order = await _vContext.Orders.AsNoTracking()
                .Where(o => o.Id == orderId)
                .Select(o => new OrderDto
                {
                    OrderId = o.Id,
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
                    TotalAmount = Math.Round(o.OrderItems
                        .Where(i => i.Status != OrderItemStatus.Cancelled)
                        .Sum(i => i.MenuItem.Price * i.Quantity), 3),
                    Status = new OrderStatusDto
                    {
                        Id = (int)o.Status,
                        Name = o.Status.GetDescription()
                    },
                    DinnerTable = new TableDto
                    {
                        Id = o.DinnerTableId
                    },
                    OrderItems = o.OrderItems
                        .Where(i =>
                            isCancelled == null ||
                            (isCancelled.Value
                                ? i.Status == OrderItemStatus.Cancelled
                                : i.Status != OrderItemStatus.Cancelled)
                        )
                        .Select(i => new OrderItemDto
                        {
                            Id = i.Id,
                            OrderId = o.Id,
                            Quantity = i.Quantity,
                            Price = i.MenuItem.Price,
                            PriceListId = i.MenuItem.PriceListId,
                            CreatedBy = i.CreatedBy,
                            CreatedDate = i.CreatedDate,
                            CancelledBy = i.CancelledBy,
                            CancelledDate = i.CancelledDate,
                            Note = i.Note,
                            KindOfHotpot1Id = i.MenuItem.KindOfHotpot1Id,
                            KindOfHotpot2Id = i.MenuItem.KindOfHotpot2Id,
                            Menu = new MenuDto
                            {
                                Id = i.MenuItem.MenuId
                            },
                            Size = new SizeDto
                            {
                                Id = i.MenuItem.SizeId
                            }
                        }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return order;
        }
    }
}
