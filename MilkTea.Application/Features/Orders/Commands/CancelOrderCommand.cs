using FluentValidation;
using MediatR;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Domain.SharedKernel.Constants;

namespace MilkTea.Application.Features.Orders.Commands;

public class CancelOrderCommand : IRequest<CancelOrderResult>
{
    public int OrderID { get; set; }
}

public sealed class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.OrderID)
            .GreaterThan(0)
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName("OrderID");
    }
}
