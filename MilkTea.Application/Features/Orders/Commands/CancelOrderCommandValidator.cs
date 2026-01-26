using FluentValidation;
using MilkTea.Application.Features.Orders.Commands;

namespace MilkTea.Application.Features.Orders.Commands;

public sealed class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.OrderID)
            .GreaterThan(0)
            .WithMessage("OrderID phải lớn hơn 0");
    }
}
