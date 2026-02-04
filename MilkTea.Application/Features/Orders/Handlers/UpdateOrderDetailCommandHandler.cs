using MediatR;
using MilkTea.Application.Features.Orders.Commands;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Shared.Extensions;

namespace MilkTea.Application.Features.Orders.Handlers
{
    public class UpdateOrderDetailCommandHandler
        (IOrderingUnitOfWork orderingUnitOfWork,
        ICurrentUser currentUser) : IRequestHandler<UpdateOrderDetailCommand, UpdateOrderDetailResult>
    {
        private readonly IOrderingUnitOfWork _vOrderingUnitOfWork = orderingUnitOfWork;
        public async Task<UpdateOrderDetailResult> Handle(UpdateOrderDetailCommand command, CancellationToken cancellationToken)
        {

            var result = new UpdateOrderDetailResult();
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
                    order.UpdateItemQuantity(command.OrderDetailID, command.Quantity.Value, currentUser.UserId);
                }

                if (command.Note is not null)
                {
                    var note = command.Note.IsNullOrWhiteSpace() ? null : command.Note;
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
