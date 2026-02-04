using FluentValidation;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands;

public class CancelOrderDetailsCommand : ICommand<CancelOrderDetailsResult>
{
    public int OrderID { get; set; }
    public List<int> OrderDetailIDs { get; set; } = new();
}

public sealed class CancelOrderDetailsCommandValidator : AbstractValidator<CancelOrderDetailsCommand>
{
    public CancelOrderDetailsCommandValidator()
    {
        // Check null, empty, and less than or equal to 0
        RuleFor(x => x.OrderID)
            .GreaterThan(0)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("OrderID");

        // check null and less than or equal to 0 if not empty
        RuleFor(x => x.OrderDetailIDs)
            .NotNull()
            .Must(list => !list.Any() || list.All(id => id > 0))
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("OrderDetailIDs");

    }
}
