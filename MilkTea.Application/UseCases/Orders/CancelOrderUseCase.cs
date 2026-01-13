using MilkTea.Application.Commands.Orders;
using MilkTea.Application.Results.Orders;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories;
using MilkTea.Domain.Respositories.Orders;

namespace MilkTea.Application.UseCases.Orders
{
    public class CancelOrderUseCase(
        IOrderRepository orderRepository,
        IStatusOfOrderRepository statusOfOrderRepository,
        IUnitOfWork unitOfWork)
    {
        private readonly IOrderRepository _vOrderRepository = orderRepository;
        private readonly IStatusOfOrderRepository _vStatusOfOrderRepository = statusOfOrderRepository;
        private readonly IUnitOfWork _vUnitOfWork = unitOfWork;

        public async Task<CancelOrderResult> Execute(CancelOrderCommand command)
        {
            var result = new CancelOrderResult();
            var pendingStatus = await _vStatusOfOrderRepository.GetPendingStatusAsync();
            if (pendingStatus is null)
                throw new InvalidDataException("NOT_PAYMENT Status not exist in the system. Should be create!");

            var cancelledStatus = await _vStatusOfOrderRepository.GetCancelledStatusAsync();
            if (cancelledStatus is null)
                throw new InvalidDataException("CANCELLED Status not exist in the system. Should be create!");

            // Get order by ID first
            var order = await _vOrderRepository.GetOrderByIdAsync(command.OrderID);
            if (order == null) return SendMessageError(result, ErrorCode.E0001, nameof(command.OrderID));

            // Check if order is in pending status (can be cancelled)
            if (order.StatusOfOrderID != pendingStatus.ID) return SendMessageError(result, ErrorCode.E0042, nameof(command.OrderID));

            await _vUnitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.UtcNow;

                // Update order
                order.StatusOfOrderID = cancelledStatus.ID;
                order.CancelledBy = command.CancelledBy;
                order.CancelledDate = now;

                if (!string.IsNullOrEmpty(command.CancelNote))
                {
                    order.Note = command.CancelNote;
                }

                // Cancel order
                var orderCancelled = await _vOrderRepository.CancelOrderAsync(order);
                if (!orderCancelled)
                {
                    await _vUnitOfWork.RollbackAsync();
                    return SendMessageError(result, ErrorCode.E0027, "Cancel Order Request");
                }

                // Cancel all order details
                var detailsCancelled = await _vOrderRepository.CancelOrderDetailsAsync(
                    order.ID,
                    command.CancelledBy,
                    now);

                if (!detailsCancelled)
                {
                    await _vUnitOfWork.RollbackAsync();
                    return SendMessageError(result, ErrorCode.E0027, "Cancel Order Details Request");
                }

                result.OrderID = order.ID;
                result.BillNo = order.BillNo;
                result.CancelledDate = now;

                await _vUnitOfWork.CommitAsync();

                return result;
            }
            catch (Exception)
            {
                await _vUnitOfWork.RollbackAsync();
                return SendMessageError(result, ErrorCode.E0027, "Cancel Order Details Request");
            }
        }

        private CancelOrderResult SendMessageError(
            CancelOrderResult result,
            string errorCode,
            params string[] values)
        {
            if (values != null && values.Length > 0)
            {
                result.ResultData.Add(errorCode, values.ToList());
            }
            return result;
        }
    }
}