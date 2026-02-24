using FluentValidation;
using MediatR;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Shared.Extensions;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands
{
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
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName("OrderID");

            RuleFor(x => x.OrderDetailID)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName("OrderDetailID");

            RuleFor(x => x)
                .Must(x => x.Quantity.HasValue || x.Note is not null)
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName("UpdateOrderDetail");
        }
    }
    public class UpdateOrderDetailCommandHandler
        (IOrderingUnitOfWork orderingUnitOfWork,
        ICurrentUser currentUser) : IRequestHandler<UpdateOrderDetailCommand, UpdateOrderDetailResult>
    {
        private readonly IOrderingUnitOfWork _vOrderingUnitOfWork = orderingUnitOfWork;
        public async Task<UpdateOrderDetailResult> Handle(UpdateOrderDetailCommand command, CancellationToken cancellationToken)
        {
            var result = new UpdateOrderDetailResult();
            // Check order exist
            var order = await _vOrderingUnitOfWork.Orders.GetOrderByIdWithItemsAsync(command.OrderID);
            if (order is null)
            {
                return SendError(result, ErrorCode.E0001, "OrderID");
            }
            await _vOrderingUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                if (command.Quantity.HasValue)
                {
                    // If quantity equals zero, remove item, else update quantity
                    if (command.Quantity.Value == 0)
                    {
                        order.CancelOrderItem(command.OrderDetailID, currentUser.UserId);
                    }
                    else
                    {
                        order.UpdateItemQuantity(command.OrderDetailID, command.Quantity.Value, currentUser.UserId);
                    }
                }
                if (command.Note is not null)
                {
                    var note = command.Note.IsNullOrWhiteSpace() ? null : command.Note.Trim();
                    order.UpdateItemNote(command.OrderDetailID, note, currentUser.UserId);
                }
                await _vOrderingUnitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch (OrderNotEditableException)
            {
                await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0042, "OrderID");
            }
            catch (OrderItemNotFoundException)
            {
                await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0001, "OrderDetailID");
            }
            catch (OrderItemCancelledException)
            {
                await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0042, "OrderDetailID");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E9999, "UpdateItemOrderDetail");
            }
            return result;
        }
        private static UpdateOrderDetailResult SendError(UpdateOrderDetailResult result, string errorCode, params string[] values)
        {
            if (values is { Length: > 0 })
                result.ResultData.Add(errorCode, values.ToList());
            return result;
        }
    }

}
