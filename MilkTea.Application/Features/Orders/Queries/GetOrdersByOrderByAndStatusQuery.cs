using MediatR;
using MilkTea.Application.Features.Orders.Results;

namespace MilkTea.Application.Features.Orders.Queries;

public sealed class GetOrdersByOrderByAndStatusQuery : IRequest<GetOrdersByOrderByAndStatusResult>
{
    public int? StatusId { get; set; }
}

