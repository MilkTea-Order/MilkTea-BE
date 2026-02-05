using MediatR;
using MilkTea.Application.Features.Orders.Results;

namespace MilkTea.Application.Features.Orders.Queries;

public sealed class GetOrderDetailByIdAndStatusQuery : IRequest<GetOrderDetailByIDAndStatusResult>
{
    public int OrderId { get; set; }
    public bool IsCancelled { get; set; } = false;
}

