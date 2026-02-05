using FluentValidation;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands
{
    public class AddOrderDetailCommand : ICommand<AddOrderDetailResult>
    {
        public int OrderID { get; set; }
        public List<OrderItemCommand> Items { get; set; } = new();
    }

    public sealed class AddOrderDetailCommandValidator : AbstractValidator<AddOrderDetailCommand>
    {
        public AddOrderDetailCommandValidator()
        {
            RuleFor(x => x.OrderID)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName("orderID");

            RuleFor(x => x.Items)
                .NotNull()
                .NotEmpty()
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName("items");
        }
    }
}