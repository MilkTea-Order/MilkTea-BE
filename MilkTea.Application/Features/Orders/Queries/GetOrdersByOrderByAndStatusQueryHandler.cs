using MediatR;
using MilkTea.Application.Features.Catalog.Abstractions;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Orders.Abstractions;
using MilkTea.Application.Features.Orders.Dtos;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Orders.Repositories;
namespace MilkTea.Application.Features.Orders.Queries;

public sealed class GetOrdersByOrderByAndStatusQuery : IRequest<GetOrdersByOrderByAndStatusResult>
{
    public int StatusId { get; set; }
    public int DayAgo { get; set; } = 0;
}

public sealed class GetOrdersByOrderByAndStatusQueryHandler(
    IOrderUnitOfWork orderingUnitOfWork,
    ICurrentUser currentUser,
    ICatalogService catalogService,
    IOrderQuery orderQuery,
    ITableService tableService) : IRequestHandler<GetOrdersByOrderByAndStatusQuery, GetOrdersByOrderByAndStatusResult>
{
    private readonly IOrderUnitOfWork _vOrderingUnitOfWork = orderingUnitOfWork;
    private readonly ICatalogService _vCatalogQuery = catalogService;
    private readonly IOrderQuery _vOrderQuery = orderQuery;
    private readonly ITableService _vTableService = tableService;
    public async Task<GetOrdersByOrderByAndStatusResult> Handle(GetOrdersByOrderByAndStatusQuery query, CancellationToken cancellationToken)
    {
        GetOrdersByOrderByAndStatusResult result = new();

        var status = Domain.Orders.Enums.OrderStatus.Unpaid;
        if (Enum.IsDefined(typeof(Domain.Orders.Enums.OrderStatus), query.StatusId))
        {
            status = (Domain.Orders.Enums.OrderStatus)query.StatusId;
        }
        var orders = await _vOrderQuery.GetOrdersAsync(currentUser.UserId, (int)status, query.DayAgo, cancellationToken);
        var tableIds = orders.Select(o => o.DinnerTableId).Distinct().ToList();

        var table = await _vTableService.GetTableAsync(tableIds, cancellationToken);
        var tableDict = table.ToDictionary(x => x.Id);

        foreach (var o in orders)
        {
            if (tableDict.TryGetValue(o.DinnerTableId, out var t))
            {
                o.DinnerTable = new TableDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Code = t.Code,
                    Position = t.Position,
                    NumberOfSeats = t.NumberOfSeats,
                    StatusId = t.StatusId,
                    StatusName = t.StatusName,
                    Note = t.Note,
                    UsingImg = t.UsingImg,
                    EmptyImg = t.EmptyImg,
                };
            }
        }
        if (query.StatusId == (int)Domain.Orders.Enums.OrderStatus.Unpaid)
        {
            orders = orders.OrderBy(o => o.DinnerTableId).ToList();
        }
        result.Orders = orders;
        return result;
    }
}
