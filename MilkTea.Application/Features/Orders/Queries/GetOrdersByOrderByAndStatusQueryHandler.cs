using MediatR;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Models.Orders;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.SharedKernel.Repositories;

namespace MilkTea.Application.Features.Orders.Queries;

public sealed class GetOrdersByOrderByAndStatusQueryHandler(
    IUnitOfWork unitOfWork,
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

        var orders = await unitOfWork.Orders.GetOrdersByOrderByAndStatusAsync(currentUser.UserId, status);
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
            TotalAmount = o.TotalAmount
        }).ToList();

        return result;
    }
}
