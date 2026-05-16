using FluentValidation;
using MilkTea.Application.Features.Orders.Models.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Common.Constants;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands
{
    public class CancelOrderDetailCommand : ICommand<CancelOrderDetailResult>
    {
        public int OrderId { get; init; }
        public int OrderDetailId { get; init; }
    }
    public sealed class CancelOrderDetailCommandValidator : AbstractValidator<CancelOrderDetailCommand>
    {
        public CancelOrderDetailCommandValidator()
        {
            // Check null, empty, and less than or equal to 0
            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName(nameof(CancelOrderDetailCommand.OrderId));

            // Check less than or equal to 0
            RuleFor(x => x.OrderDetailId)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName(nameof(CancelOrderDetailCommand.OrderDetailId));

        }
    }
    public class CancelOrderDetailCommandHandler(
                                IOrderUnitOfWork orderingUnitOfWork,
                                IIdentifyServicePorts currentUser) : ICommandHandler<CancelOrderDetailCommand, CancelOrderDetailResult>
    {
        private readonly IOrderUnitOfWork _vOrderingUnitOfWork = orderingUnitOfWork;
        public async Task<CancelOrderDetailResult> Handle(CancelOrderDetailCommand command, CancellationToken cancellationToken)
        {
            var result = new CancelOrderDetailResult();
            // Load order with items for update
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
                var cancelledBy = currentUser.UserId;
                order.CancelOrderItem(command.OrderDetailId, cancelledBy);
                await _vOrderingUnitOfWork.CommitTransactionAsync(cancellationToken);
                return result;
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
                return SendError(result, ErrorCode.E9999, "CancelOrderDetail");
            }
        }

        private static CancelOrderDetailResult SendError(CancelOrderDetailResult result, string errorCode, params string[] values)
        {
            if (values is { Length: > 0 })
                result.ResultData.Add(errorCode, values.ToList());
            return result;
        }

    }

}
