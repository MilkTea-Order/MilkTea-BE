using FluentValidation;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands
{
    public class CancelOrderDetailCommnad : ICommand<CancelOrderDetailResult>
    {
        public int OrderID { get; set; }
        public int OrderDetailID { get; set; }
    }
    public sealed class CancelOrderDetailCommnadValidator : AbstractValidator<CancelOrderDetailCommnad>
    {
        public CancelOrderDetailCommnadValidator()
        {
            // Check null, empty, and less than or equal to 0
            RuleFor(x => x.OrderID)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName("OrderID");

            // Check less than or equal to 0
            RuleFor(x => x.OrderDetailID)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName("OrderDetailID");

        }
    }
}
