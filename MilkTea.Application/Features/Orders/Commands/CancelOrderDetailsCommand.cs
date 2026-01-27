using FluentValidation;
using MediatR;
using MilkTea.Application.Features.Orders.Results;

namespace MilkTea.Application.Features.Orders.Commands;

public class CancelOrderDetailsCommand : IRequest<CancelOrderDetailsResult>
{
    public int OrderID { get; set; }
    public List<int> OrderDetailIDs { get; set; } = new();
}

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
