using FluentValidation;
using MediatR;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Orders.Abstractions;
using MilkTea.Application.Features.Orders.Models.Dtos;
using MilkTea.Application.Features.Orders.Models.Results;
using MilkTea.Domain.Common.Constants;
using MilkTea.Domain.Orders.Enums;
using MilkTea.Shared.Extensions;
using Shared.Abstractions.CQRS;
using Shared.Extensions;

namespace MilkTea.Application.Features.Orders.Queries;


public sealed class GetKitchenOrdersQuery : IQuery<GetKitchenOrdersResult>
{
    public int OrderStatusId { get; set; } = (int)OrderStatus.Unpaid;
    public string OrderItemStatus { get; set; } = string.Empty;
}

public sealed class GetKitchenOrdersQueryValidator : AbstractValidator<GetKitchenOrdersQuery>
{
    public GetKitchenOrdersQueryValidator()
    {
        RuleFor(x => x.OrderStatusId)
            .Must(x => Enum.IsDefined(typeof(OrderStatus), x))
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName(nameof(GetKitchenOrdersQuery.OrderStatusId));

        RuleFor(x => x.OrderItemStatus)
            .Must(statusName => string.IsNullOrEmpty(statusName) 
                                || OrderDetailStatusExtensions.TryParse(statusName, out _))
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName(nameof(GetKitchenOrdersQuery.OrderItemStatus));
    }
}

public sealed class GetKitchenOrdersQueryHandler(IOrderQuery orderQuery,
                                                    ICatalogService catalogService,
                                                    ITableService tableService) : IRequestHandler<GetKitchenOrdersQuery, GetKitchenOrdersResult>
{
    private readonly IOrderQuery _vOrderQuery = orderQuery;
    private readonly ICatalogService _vCatalogService = catalogService;
    private readonly ITableService _vTableService = tableService;

    public async Task<GetKitchenOrdersResult> Handle(GetKitchenOrdersQuery query, CancellationToken cancellationToken)
    {
        var result = new GetKitchenOrdersResult();

        var orderStatus = (OrderStatus)query.OrderStatusId;
        var orderDetailStatus = query.OrderItemStatus.IsNullOrWhiteSpace() 
                                    ? OrderItemStatus.Pending 
                                    : Enum.Parse<OrderItemStatus>(query.OrderItemStatus, ignoreCase: true);

        var orders = await _vOrderQuery.GetKitchenOrdersAsync(orderStatus, orderDetailStatus, cancellationToken);

        if (orders.Count == 0)
        {
            result.Orders = orders;
            return result;
        }

        var tableIds = orders.Select(o => o.DinnerTable!.Id).Distinct().ToList();
        var menuIds = orders.SelectMany(o => o.OrderItems).Select(i => i.Menu == null ? 0 : i.Menu.Id).Where(id => id != 0).Distinct().ToList();
        var sizeIds = orders.SelectMany(o => o.OrderItems).Select(i => i.Size == null ? 0 : i.Size.Id).Where(id => id != 0).Distinct().ToList();

        var tables = await _vTableService.GetTableAsync(tableIds, cancellationToken);
        var tableDict = tables.ToDictionary(t => t.Id);

        var menus = await _vCatalogService.GetMenusAsync(menuIds, cancellationToken);
        var sizes = await _vCatalogService.GetMenuSizesAsync(sizeIds, cancellationToken);

        foreach (var order in orders)
        {
            if (tableDict.TryGetValue(order.DinnerTable!.Id, out var table))
            {
                order.DinnerTable = new TableDto
                {
                    Id = table.Id,
                    Name = table.Name,
                    Code = table.Code,
                    Position = table.Position,
                    NumberOfSeats = table.NumberOfSeats,
                    StatusId = table.StatusId,
                    StatusName = table.StatusName,
                    Note = table.Note,
                    UsingImg = table.UsingImg,
                    EmptyImg = table.EmptyImg,
                };
            }

            foreach (var item in order.OrderItems)
            {
                if (menus.TryGetValue(item.Menu?.Id ?? 0, out var menu))
                {
                    item.Menu = new MenuDto
                    {
                        Id = menu.MenuId,
                        Code = menu.MenuCode,
                        Name = menu.MenuName,
                        MenuGroupId = menu.MenuGroupId,
                        MenuGroupName = menu.MenuGroupName,
                        StatusId = menu.StatusId,
                        StatusName = menu.StatusName,
                        UnitId = menu.UnitId,
                        UnitName = menu.UnitName,
                        Note = menu.Note,
                    };
                }

                if (sizes.TryGetValue(item.Size?.Id ?? 0, out var size))
                {
                    item.Size = new SizeDto
                    {
                        Id = size.SizeId,
                        Name = size.SizeName,
                        RankIndex = size.RankIndex,
                    };
                }
            }
        }

        result.Orders = sortOrdersByStatus(orders, orderDetailStatus);
        return result;
    }

    private static List<KitchenOrderDto> sortOrdersByStatus(List<KitchenOrderDto> orders,OrderItemStatus status)
    {
        return status switch
        {
            OrderItemStatus.Pending =>
                orders.OrderBy(o => o.DinnerTable!.Id)
                      .ThenBy(o => o.CreatedDate)
                      .ToList(),

            OrderItemStatus.InProgress =>
                orders.OrderBy(o => o.DinnerTable!.Id)
                      .ThenBy(o => o.OrderItems
                          .Where(i => i.PerformDate.HasValue)
                          .MinBy(i => i.PerformDate)?.PerformDate ?? DateTime.MaxValue)
                      .ToList(),

            OrderItemStatus.Completed =>
                orders.OrderBy(o => o.DinnerTable!.Id)
                      .ThenByDescending(o => o.OrderItems
                          .Where(i => i.CompletedDate.HasValue)
                          .MaxBy(i => i.CompletedDate)?.CompletedDate ?? DateTime.MinValue)
                      .ToList(),

            OrderItemStatus.Cancelled =>
                orders.OrderBy(o => o.DinnerTable!.Id)
                      .ThenByDescending(o => o.OrderItems
                          .Where(i => i.CancelledDate.HasValue)
                          .MaxBy(i => i.CancelledDate)?.CancelledDate ?? DateTime.MinValue)
                      .ToList(),

            _ => orders.OrderBy(o => o.DinnerTable!.Id).ThenBy(o => o.CreatedDate).ToList()
        };
    }
}
