using MediatR;
using MilkTea.Application.Features.Catalog.Abstractions;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Models.Orders;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Orders.Repositories;
using Shared.Extensions;
namespace MilkTea.Application.Features.Orders.Queries;

public sealed class GetOrdersByOrderByAndStatusQuery : IRequest<GetOrdersByOrderByAndStatusResult>
{
    public int StatusId { get; set; }
}

public sealed class GetOrdersByOrderByAndStatusQueryHandler(
    IOrderingUnitOfWork orderingUnitOfWork,
    ICurrentUser currentUser,
    ICatalogService catalogService) : IRequestHandler<GetOrdersByOrderByAndStatusQuery, GetOrdersByOrderByAndStatusResult>
{
    private readonly IOrderingUnitOfWork _vOrderingUnitOfWork = orderingUnitOfWork;
    private readonly ICatalogService _vCatalogQuery = catalogService;
    public async Task<GetOrdersByOrderByAndStatusResult> Handle(GetOrdersByOrderByAndStatusQuery query, CancellationToken cancellationToken)
    {
        var result = new GetOrdersByOrderByAndStatusResult();

        var status = Domain.Orders.Enums.OrderStatus.Unpaid;
        if (Enum.IsDefined(typeof(Domain.Orders.Enums.OrderStatus), query.StatusId))
        {
            status = (Domain.Orders.Enums.OrderStatus)query.StatusId;
        }
        var orders = await _vOrderingUnitOfWork.Orders.GetOrdersByOrderByAndStatusWithItemsAsync(currentUser.UserId, status);
        var tableIds = orders.Select(o => o.DinnerTableId).Distinct().ToList();
        var tableDict = await _vCatalogQuery.GetTableAsync(tableIds, cancellationToken);

        result.Orders = orders.Select(o =>
            new Order
            {
                OrderId = o.Id,
                DinnerTableId = o.DinnerTableId,
                OrderDate = o.OrderDate,
                OrderBy = o.OrderBy,
                CreatedDate = o.CreatedDate,
                CreatedBy = o.CreatedBy,
                StatusId = (int)o.Status,
                Note = o.Note,
                TotalAmount = o.GetTotalAmount(),
                Status = new OrderStatus
                {
                    Id = (int)o.Status,
                    Name = o.Status.GetDescription()
                },
                DinnerTable = tableDict.TryGetValue(o.DinnerTableId, out var table)
                    ? new Table
                    {
                        Id = table.Id,
                        Name = table.Name,
                        Code = table.Code,
                        StatusId = table.StatusId,
                        StatusName = table.StatusName,
                        NumberOfSeats = table.NumberOfSeats,
                        UsingImg = table.UsingImg,
                    }
                    : null
            }
        ).OrderBy(o => o.DinnerTableId).ToList();
        return result;
    }
}
