using FluentValidation;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands;

public class UpdateOrderDetailCommand : ICommand<UpdateOrderDetailResult>
{
    public int OrderID { get; set; }
    public int OrderDetailID { get; set; }
    public int? Quantity { get; set; }
    public string? Note { get; set; }
}

public sealed class UpdateOrderDetailCommandValidator : AbstractValidator<UpdateOrderDetailCommand>
{
    public UpdateOrderDetailCommandValidator()
    {
        RuleFor(x => x.OrderID)
            .GreaterThan(0)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("OrderID");

        RuleFor(x => x.OrderDetailID)
            .GreaterThan(0)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("OrderDetailID");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .When(x => x.Quantity.HasValue)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("Quantity");

        RuleFor(x => x.Note)
            .MaximumLength(200)
            .When(x => x.Note is not null)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("Note");

        RuleFor(x => x)
            .Must(x => x.Quantity.HasValue || x.Note is not null)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("UpdateOrderDetail");
    }
}
