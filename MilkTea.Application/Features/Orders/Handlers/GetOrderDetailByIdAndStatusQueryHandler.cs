using MediatR;
using MilkTea.Application.Features.Catalog.Services;
using MilkTea.Application.Features.Orders.Queries;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Models.Orders;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Extensions;

namespace MilkTea.Application.Features.Orders.Handlers;

public sealed class GetOrderDetailByIdAndStatusQueryHandler(
    IOrderingUnitOfWork orderingUnitOfWork,
    ICatalogService catalogService) : IRequestHandler<GetOrderDetailByIdAndStatusQuery, GetOrderDetailByIDAndStatusResult>
{
    private readonly ICatalogService _vCatalogQuery = catalogService;
    private readonly IOrderingUnitOfWork _vOrderUnitOfWork = orderingUnitOfWork;
    public async Task<GetOrderDetailByIDAndStatusResult> Handle(GetOrderDetailByIdAndStatusQuery query, CancellationToken cancellationToken)
    {
        var result = new GetOrderDetailByIDAndStatusResult();

        if (query.OrderId <= 0)
        {
            return SendError(result, ErrorCode.E0036, nameof(query.OrderId));
        }

        var order = await _vOrderUnitOfWork.Orders.GetOrderDetailByIDAndStatus(query.OrderId, null);

        if (order is null)
        {
            return SendError(result, ErrorCode.E0001, nameof(query.OrderId));
        }

        var orderItems = order.OrderItems.Where(x => x.IsCancelled == query.IsCancelled).ToList();

        var menuIds = orderItems.Select(x => x.MenuItem.MenuId).Distinct();
        var sizeIds = orderItems.Select(x => x.MenuItem.SizeId).Distinct();

        var menus = await _vCatalogQuery.GetMenusAsync(menuIds, cancellationToken);
        var sizes = await _vCatalogQuery.GetMenuSizesAsync(sizeIds, cancellationToken);
        var table = await _vCatalogQuery.GetTableAsync(order.DinnerTableId, cancellationToken);

        result.Order = new OrderDetail
        {
            OrderId = order.Id,
            DinnerTableId = order.DinnerTableId,
            OrderDate = order.OrderDate,
            OrderBy = order.OrderBy,
            CreatedDate = order.CreatedDate,
            CreatedBy = order.CreatedBy,
            StatusId = (int)order.Status,
            Note = order.Note,
            TotalAmount = order.GetTotalAmount(),
            Status = new OrderStatus
            {
                Id = (int)order.Status,
                Name = order.Status.GetDescription()
            },
            DinnerTable = table is null ? null : new Table
            {
                Id = table.Id,
                Code = table.Code,
                Name = table.Name,
                Position = table.Position,
                NumberOfSeats = table.NumberOfSeats,
                StatusId = table.StatusId,
                StatusName = table.StatusName,
                Note = table.Note,
            },
            OrderDetails = orderItems
                .Select(item => new OrderLine
                {
                    Id = item.Id,
                    OrderId = order.Id,
                    MenuId = item.MenuItem.MenuId,
                    SizeId = item.MenuItem.SizeId,
                    Quantity = item.Quantity,
                    Price = item.MenuItem.Price,
                    PriceListId = item.MenuItem.PriceListId,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    CancelledBy = item.CancelledBy,
                    CancelledDate = item.CancelledDate,
                    Note = item.Note,
                    KindOfHotpot1Id = item.MenuItem.KindOfHotpot1Id,
                    KindOfHotpot2Id = item.MenuItem.KindOfHotpot2Id,
                    Menu = menus.TryGetValue(item.MenuItem.MenuId, out var m)
                                    ? new Menu
                                    {
                                        Id = m.MenuId,
                                        Code = m.MenuCode,
                                        Name = m.MenuName,
                                        MenuGroupName = m.MenuGroupName,
                                        StatusId = m.StatusId,
                                        StatusName = m.StatusName,
                                        UnitId = m.UnitId,
                                        UnitName = m.UnitName,
                                        Note = m.Note
                                    }
                                    : null,

                    Size = sizes.TryGetValue(item.MenuItem.SizeId, out var s)
                                    ? new Size
                                    {
                                        Id = s.SizeId,
                                        Name = s.SizeName,
                                        RankIndex = s.RankIndex
                                    }
                                    : null,
                }).ToList()
        };
        return result;
    }

    private static GetOrderDetailByIDAndStatusResult SendError(GetOrderDetailByIDAndStatusResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
