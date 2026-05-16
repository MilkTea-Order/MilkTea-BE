using FluentValidation;
using MediatR;
using MilkTea.Application.Features.Orders.Models.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Common.Constants;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Shared.Extensions;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands
{
    public class UpdateOrderItemCommand : ICommand<UpdateOrderItemResult>
    {
        public int OrderId { get; init; }
        public int OrderDetailId { get; init; }
        public int? Quantity { get; init; }
        public string? Note { get; init; }
    }

    public sealed class UpdateOrderItemCommandValidator : AbstractValidator<UpdateOrderItemCommand>
    {
        public UpdateOrderItemCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName(nameof(UpdateOrderItemCommand.OrderId));

            RuleFor(x => x.OrderDetailId)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName(nameof(UpdateOrderItemCommand.OrderDetailId));

            RuleFor(x => x)
                .Must(x => x.Quantity.HasValue || x.Note is not null)
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName("UpdateOrderDetail");
        }
    }
    public class UpdateOrderDetailCommandHandler
        (IOrderUnitOfWork orderingUnitOfWork,
        IIdentifyServicePorts currentUser) : IRequestHandler<UpdateOrderItemCommand, UpdateOrderItemResult>
    {
        private readonly IOrderUnitOfWork _vOrderingUnitOfWork = orderingUnitOfWork;
        public async Task<UpdateOrderItemResult> Handle(UpdateOrderItemCommand command, CancellationToken cancellationToken)
        {
            var result = new UpdateOrderItemResult();
            // Check order exist
            var order = await _vOrderingUnitOfWork.Orders.GetOrderByIdWithItemIdAsync(command.OrderId, 
                                                                                        command.OrderDetailId, 
                                                                                        cancellationToken);
            if (order is null)
            {
                return SendError(result, ErrorCode.E0001, nameof(command.OrderId));
            }
            await _vOrderingUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                if (command.Quantity.HasValue)
                {
                    // If quantity equals zero, remove item, else update quantity
                    if (command.Quantity.Value == 0)
                    {
                        order.CancelOrderItem(command.OrderDetailId, currentUser.UserId);
                    }
                    else
                    {
                        order.UpdateOrderItemQuantity(command.OrderDetailId, command.Quantity.Value, currentUser.UserId);
                    }
                }
                if (command.Note is not null)
                {
                    var note = command.Note.IsNullOrWhiteSpace() ? null : command.Note.Trim();
                    order.UpdateOrderItemNote(command.OrderDetailId, note, currentUser.UserId);
                }
                await _vOrderingUnitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch (OrderNotEditableException)
            {
                await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0042, nameof(command.OrderId));
            }
            catch (OrderItemNotFoundException)
            {
                await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0001, nameof(command.OrderDetailId));
            }
            catch (OrderItemStatusInValidException)
            {
                await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0042, "OrderItemInvalidStatusToCancelItem");
            }
            catch (Exception)
            {
                await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E9999, "UpdateOrderItem");
            }
            return result;
        }
        private static UpdateOrderItemResult SendError(UpdateOrderItemResult result, string errorCode, params string[] values)
        {
            if (values is { Length: > 0 })
                result.ResultData.Add(errorCode, values.ToList());
            return result;
        }
    }

}
