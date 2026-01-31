using FluentValidation;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands;

public class CreateOrderCommand : ICommand<CreateOrderResult>
{
    public int DinnerTableID { get; set; }
    public List<OrderItemCommand> Items { get; set; } = new();
    public int? OrderedBy { get; set; }
    public string? Note { get; set; }
}

public class OrderItemCommand
{
    public int MenuID { get; set; }
    public int SizeID { get; set; }
    public int Quantity { get; set; }
    public List<int>? ToppingIDs { get; set; }
    public List<int>? KindOfHotpotIDs { get; set; }
    public string? Note { get; set; }
}

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.DinnerTableID)
            .GreaterThan(0)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("dinnerTableId");

        RuleFor(x => x.Items)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("items");
        //.DependentRules(() =>
        //{
        //    RuleForEach(x => x.Items)
        //        .ChildRules(item =>
        //        {
        //            item.RuleFor(i => i.MenuID)
        //                .GreaterThan(0)
        //                .WithMessage("MenuID phải lớn hơn 0");

        //            item.RuleFor(i => i.SizeID)
        //                .GreaterThan(0)
        //                .WithMessage("SizeID phải lớn hơn 0");

        //            item.RuleFor(i => i.Quantity)
        //                .GreaterThan(0)
        //                .WithMessage("Quantity phải lớn hơn 0");
        //        });
        //});

        RuleFor(x => x.OrderedBy)
            .GreaterThan(0)
            .When(x => x.OrderedBy.HasValue)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("orderedBy");
    }
}
