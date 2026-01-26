using FluentValidation;
using MilkTea.Application.Features.Orders.Commands;

namespace MilkTea.Application.Features.Orders.Commands;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.DinnerTableID)
            .GreaterThan(0)
            .WithMessage("DinnerTableID phải lớn hơn 0");

        RuleFor(x => x.Items)
            .NotNull()
            .NotEmpty()
            .WithMessage("Items không được để trống")
            .DependentRules(() =>
            {
                RuleForEach(x => x.Items)
                    .ChildRules(item =>
                    {
                        item.RuleFor(i => i.MenuID)
                            .GreaterThan(0)
                            .WithMessage("MenuID phải lớn hơn 0");

                        item.RuleFor(i => i.SizeID)
                            .GreaterThan(0)
                            .WithMessage("SizeID phải lớn hơn 0");

                        item.RuleFor(i => i.Quantity)
                            .GreaterThan(0)
                            .WithMessage("Quantity phải lớn hơn 0");
                    });
            });

        RuleFor(x => x.OrderedBy)
            .GreaterThan(0)
            .When(x => x.OrderedBy.HasValue)
            .WithMessage("OrderedBy phải lớn hơn 0 nếu được cung cấp");
    }
}
