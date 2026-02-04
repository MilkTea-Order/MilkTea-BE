using MediatR;
using MilkTea.Application.Features.Orders.Queries;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Models.Orders;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Orders.Repositories;

namespace MilkTea.Application.Features.Orders.Handlers;

public sealed class GetOrdersByOrderByAndStatusQueryHandler(
    IOrderingUnitOfWork orderingUnitOfWork,
    ICurrentUser currentUser) : IRequestHandler<GetOrdersByOrderByAndStatusQuery, GetOrdersByOrderByAndStatusResult>
{
    public async Task<GetOrdersByOrderByAndStatusResult> Handle(GetOrdersByOrderByAndStatusQuery query, CancellationToken cancellationToken)
    {
        var result = new GetOrdersByOrderByAndStatusResult();

        Domain.Orders.Enums.OrderStatus? status = null;
        if (query.StatusId.HasValue && Enum.IsDefined(typeof(Domain.Orders.Enums.OrderStatus), query.StatusId.Value))
        {
            status = (Domain.Orders.Enums.OrderStatus)query.StatusId.Value;
        }

        var orders = await orderingUnitOfWork.Orders.GetOrdersByOrderByAndStatusWithRelationshipAsync(currentUser.UserId, status);
        result.Orders = orders.Select(o => new Order
        {
            OrderId = o.Id,
            DinnerTableId = o.DinnerTableId,
            OrderDate = o.OrderDate,
            OrderBy = o.OrderBy,
            CreatedDate = o.CreatedDate,
            CreatedBy = o.CreatedBy,
            StatusId = (int)o.Status,
            Note = o.Note,
            TotalAmount = o.GetTotalAmount()
        }).ToList();

        return result;
    }
}
