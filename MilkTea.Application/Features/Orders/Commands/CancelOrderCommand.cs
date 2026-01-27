using FluentValidation;
using MediatR;
using MilkTea.Application.Features.Orders.Results;

namespace MilkTea.Application.Features.Orders.Commands;

public class CancelOrderCommand : IRequest<CancelOrderResult>
{
    public int OrderID { get; set; }
    public string? CancelNote { get; set; }
}

public sealed class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.OrderID)
            .GreaterThan(0)
            .WithMessage("OrderID phải lớn hơn 0");
    }
}
