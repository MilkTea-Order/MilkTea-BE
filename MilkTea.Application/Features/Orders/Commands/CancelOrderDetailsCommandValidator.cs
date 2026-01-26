using FluentValidation;
using MilkTea.Application.Features.Orders.Commands;

namespace MilkTea.Application.Features.Orders.Commands;

public sealed class CancelOrderDetailsCommandValidator : AbstractValidator<CancelOrderDetailsCommand>
{
    public CancelOrderDetailsCommandValidator()
    {
        RuleFor(x => x.OrderID)
            .GreaterThan(0)
            .WithMessage("OrderID phải lớn hơn 0");

        RuleFor(x => x.OrderDetailIDs)
            .NotNull()
            .WithMessage("OrderDetailIDs không được null");
    }
}
