using MediatR;
using MilkTea.Application.DTOs.Orders;
using MilkTea.Application.Ports.Users;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Domain.Orders.Repositories;

namespace MilkTea.Application.Features.Orders.Queries;

public sealed class GetOrdersByOrderByAndStatusQueryHandler(
    IOrderRepository orderRepository,
    ICurrentUser currentUser) : IRequestHandler<GetOrdersByOrderByAndStatusQuery, GetOrdersByOrderByAndStatusResult>
{
    public async Task<GetOrdersByOrderByAndStatusResult> Handle(GetOrdersByOrderByAndStatusQuery query, CancellationToken cancellationToken)
    {
        var result = new GetOrdersByOrderByAndStatusResult();

        // Convert StatusId to enum if provided
        Domain.Orders.Enums.OrderStatus? status = null;
        if (query.StatusId.HasValue && Enum.IsDefined(typeof(Domain.Orders.Enums.OrderStatus), query.StatusId.Value))
        {
            status = (Domain.Orders.Enums.OrderStatus)query.StatusId.Value;
        }

        var orders = await orderRepository.GetOrdersByOrderByAndStatusAsync(currentUser.UserId, status);
        result.Orders = orders.Select(o => new OrderDto
        {
            OrderId = o.Id,
            DinnerTableId = o.DinnerTableId,
            OrderDate = o.OrderDate,
            OrderBy = o.OrderBy,
            CreatedDate = o.CreatedDate,
            CreatedBy = o.CreatedBy,
            StatusId = (int)o.Status,
            Note = o.Note,
            TotalAmount = o.TotalAmount
        }).ToList();

        return result;
    }
}
