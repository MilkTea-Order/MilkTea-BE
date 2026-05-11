using MediatR;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Orders.Models.Dtos;
using MilkTea.Application.Features.Orders.Models.Results;
using MilkTea.Domain.Common.Constants;
using MilkTea.Domain.Orders.Repositories;
using Shared.Extensions;

namespace MilkTea.Application.Features.Orders.Queries;


public sealed class GetOrderDetailByIdAndStatusQuery : IRequest<GetOrderDetailByIDAndStatusResult>
{
    public int OrderId { get; set; }
    public bool IsCancelled { get; set; } = false;
}

public sealed class GetOrderDetailByIdAndStatusQueryHandler(
    IOrderUnitOfWork orderingUnitOfWork,
    ICatalogService catalogService) : IRequestHandler<GetOrderDetailByIdAndStatusQuery, GetOrderDetailByIDAndStatusResult>
{
    private readonly ICatalogService _vCatalogQuery = catalogService;
    private readonly IOrderUnitOfWork _vOrderUnitOfWork = orderingUnitOfWork;
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

        result.Order = new OrderDto
        {
            OrderId = order.Id,
            DinnerTableId = order.DinnerTableId,
            OrderDate = order.OrderDate,
            OrderBy = order.OrderBy,
            CreatedDate = order.CreatedDate,
            CreatedBy = order.CreatedBy,
            Note = order.Note,
            TotalAmount = order.GetTotalAmount(),
            Status = new OrderStatusDto
            {
                Id = (int)order.Status,
                Name = order.Status.GetDescription()
            },
            DinnerTable = table is null ? null : new TableDto
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
            OrderItems = orderItems
                .Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    OrderId = order.Id,
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
                                    ? new MenuDto
                                    {
                                        Id = m.MenuId,
                                        Code = m.MenuCode,
                                        Name = m.MenuName,
                                        MenuGroupId = m.MenuGroupId,
                                        MenuGroupName = m.MenuGroupName,
                                        StatusId = m.StatusId,
                                        StatusName = m.StatusName,
                                        UnitId = m.UnitId,
                                        UnitName = m.UnitName,
                                        Note = m.Note
                                    }
                                    : null,

                    Size = sizes.TryGetValue(item.MenuItem.SizeId, out var s)
                                    ? new SizeDto
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
