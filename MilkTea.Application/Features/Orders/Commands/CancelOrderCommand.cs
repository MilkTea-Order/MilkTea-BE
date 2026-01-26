using MediatR;
using MilkTea.Application.Features.Orders.Results;

namespace MilkTea.Application.Features.Orders.Commands;

public class CancelOrderCommand : IRequest<CancelOrderResult>
{
    public int OrderID { get; set; }
    public string? CancelNote { get; set; }
}
