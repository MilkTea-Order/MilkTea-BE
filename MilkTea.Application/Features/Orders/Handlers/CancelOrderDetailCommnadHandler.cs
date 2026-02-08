using MilkTea.Application.Features.Orders.Commands;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Handlers
{
    public class CancelOrderDetailCommnadHandler(
                                        IOrderingUnitOfWork orderingUnitOfWork,
                                    ICurrentUser currentUser) : ICommandHandler<CancelOrderDetailCommnad, CancelOrderDetailResult>
    {
        private readonly IOrderingUnitOfWork _vOrderingUnitOfWork = orderingUnitOfWork;
        public async Task<CancelOrderDetailResult> Handle(CancelOrderDetailCommnad command, CancellationToken cancellationToken)
        {
            var result = new CancelOrderDetailResult();
            // Load order with items for update
            var order = await _vOrderingUnitOfWork.Orders.GetOrderByIdWithItemsAsync(command.OrderID);
            if (order is null)
            {
                return SendError(result, ErrorCode.E0001, "OrderID");
            }

            await _vOrderingUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var cancelledBy = currentUser.UserId;
                order.CancelOrderItem(command.OrderDetailID, cancelledBy);
                await _vOrderingUnitOfWork.CommitTransactionAsync(cancellationToken);
                return result;
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
