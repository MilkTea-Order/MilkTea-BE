using MediatR;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Orders.Abstractions;
using MilkTea.Application.Features.Orders.Models.Results;
using MilkTea.Domain.Common.Constants;

namespace MilkTea.Application.Features.Orders.Queries;


public sealed class GetOrderDetailByIdAndStatusQuery : IRequest<GetOrderDetailByIDAndStatusResult>
{
    public int OrderId { get; init; }
    public bool IsCancelled { get; init; }
}

public sealed class GetOrderDetailByIdAndStatusQueryHandler(
    IOrderQuery orderQuery,
    ICatalogService catalogService) : IRequestHandler<GetOrderDetailByIdAndStatusQuery, GetOrderDetailByIDAndStatusResult>
{
    private readonly ICatalogService _vCatalogQuery = catalogService;
    private readonly IOrderQuery _vOrderQuery = orderQuery;
    public async Task<GetOrderDetailByIDAndStatusResult> Handle(GetOrderDetailByIdAndStatusQuery query, CancellationToken cancellationToken)
    {
        var result = new GetOrderDetailByIDAndStatusResult();

        if (query.OrderId <= 0)
        {
            return SendError(result, ErrorCode.E0036, nameof(query.OrderId));
        }

        var order = await _vOrderQuery.GetOrderDetailByIdAndStatusAsync(query.OrderId, query.IsCancelled, cancellationToken);

        if (order is null)
        {
            return SendError(result, ErrorCode.E0001, nameof(query.OrderId));
        }

        var menuIds = order.OrderItems.Select(x => x.Menu?.Id).Where(id => id.HasValue).Select(id => id!.Value).Distinct();
        var sizeIds = order.OrderItems.Select(x => x.Size?.Id).Where(id => id.HasValue).Select(id => id!.Value).Distinct();
        var menus = await _vCatalogQuery.GetMenusAsync(menuIds, cancellationToken);
        var sizes = await _vCatalogQuery.GetMenuSizesAsync(sizeIds, cancellationToken);
        var table = await _vCatalogQuery.GetTableAsync(order.DinnerTable!.Id, cancellationToken);
        
        foreach (var item in order.OrderItems)
        {
            if (item.Menu != null && menus.TryGetValue(item.Menu.Id, out var m))
            {
                item.Menu.MenuGroupId = m.MenuGroupId;
                item.Menu.MenuGroupName = m.MenuGroupName;
                item.Menu.Code = m.MenuCode;
                item.Menu.Name = m.MenuName;
                item.Menu.StatusId = m.StatusId;
                item.Menu.StatusName = m.StatusName;
                item.Menu.UnitId = m.UnitId;
                item.Menu.UnitName = m.UnitName;
            }

            if (item.Size != null && sizes.TryGetValue(item.Size.Id, out var s))
            {
                item.Size.Name = s.SizeName;
                item.Size.RankIndex = s.RankIndex;
            }
        }

        if (order.DinnerTable != null && table != null)
        {
            order.DinnerTable.Code = table.Code;
            order.DinnerTable.Name = table.Name;
            order.DinnerTable.Position = table.Position;
            order.DinnerTable.NumberOfSeats = table.NumberOfSeats;
            order.DinnerTable.StatusId = table.StatusId;
            order.DinnerTable.StatusName = table.StatusName;
            order.DinnerTable.Note = table.Note;
        }

        result.Order = order;
        return result;
    }

    private static GetOrderDetailByIDAndStatusResult SendError(GetOrderDetailByIDAndStatusResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
