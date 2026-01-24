using MilkTea.Application.Commands.Orders;
using MilkTea.Application.Results.Orders;
using MilkTea.Application.Ports.Identity;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories;
using MilkTea.Domain.Respositories.Orders;

namespace MilkTea.Application.UseCases.Orders
{
    public class CancelOrderDetailsUseCase(
                            IOrderRepository orderRepository,
                            IStatusOfOrderRepository statusOfOrderRepository,
                            IUnitOfWork unitOfWork,
                            ICurrentUser currentUser)
    {
        private readonly IOrderRepository _vOrderRepository = orderRepository;
        private readonly IStatusOfOrderRepository _vStatusOfOrderRepository = statusOfOrderRepository;
        private readonly IUnitOfWork _vUnitOfWork = unitOfWork;
        private readonly ICurrentUser _currentUser = currentUser;

        public async Task<CancelOrderDetailsResult> Execute(CancelOrderDetailsCommand command)
        {
            var result = new CancelOrderDetailsResult();

            // Get pending status
            var pendingStatus = await _vStatusOfOrderRepository.GetPendingStatusAsync();
            if (pendingStatus is null)
                throw new InvalidDataException("PENDING Status not exist in the system. Should be create!");

            // Validate order exists
            var order = await _vOrderRepository.GetOrderByIdAsync(command.OrderID);
            if (order is null) return SendMessageError(result, ErrorCode.E0001, nameof(command.OrderID));

            // Check if order is in pending status (can cancel items)
            if (order.StatusOfOrderID != pendingStatus.ID)
            {
                return SendMessageError(result, ErrorCode.E0042, "Order");
            }

            // If no order detail IDs provided, return empty result
            if (command.OrderDetailIDs is null || command.OrderDetailIDs.Count == 0) return result;

            // Validate order detail IDs belong to this order
            var validOrderDetailIds = await _vOrderRepository.GetOrderDetailIdsByOrderIdAsync(command.OrderID);
            var invalidDetailIds = command.OrderDetailIDs
                                                .Where(id => !validOrderDetailIds.Contains(id))
                                                .ToList();

            if (invalidDetailIds.Count > 0) return SendMessageError(result, ErrorCode.E0001, "OrderDetailIDs");

            // Check if any order detail is already cancelled
            var alreadyCancelledIds = new List<int>();
            foreach (var detailId in command.OrderDetailIDs)
            {
                var isCancelled = await _vOrderRepository.IsOrderDetailCancelledAsync(detailId);
                if (isCancelled)
                {
                    alreadyCancelledIds.Add(detailId);
                }
            }

            if (alreadyCancelledIds.Count > 0)
            {
                return SendMessageError(result, ErrorCode.E0029, $"OrderDetailIDs: {string.Join(", ", alreadyCancelledIds)}");
            }

            await _vUnitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.UtcNow;
                var cancelledBy = _currentUser.UserId;
                var successfullyCancelledIds = new List<int>();

                foreach (var detailId in command.OrderDetailIDs)
                {
                    var cancelled = await _vOrderRepository.CancelOrderDetailAsync(
                        detailId,
                        cancelledBy,
                        now);

                    if (cancelled) successfullyCancelledIds.Add(detailId);

                }

                if (successfullyCancelledIds.Count == 0)
                {
                    await _vUnitOfWork.RollbackAsync();
                    return SendMessageError(result, ErrorCode.E0027, "Cancel Order Details");
                }

                result.OrderID = order.ID;
                result.CancelledDetailIDs = successfullyCancelledIds;
                result.CancelledDate = now;
                await _vUnitOfWork.CommitAsync();
                return result;
            }
            catch (Exception)
            {
                await _vUnitOfWork.RollbackAsync();
                return SendMessageError(result, ErrorCode.E0027, "Cancel Order Details");
            }
        }

        private CancelOrderDetailsResult SendMessageError(
            CancelOrderDetailsResult result,
            string errorCode,
            params string[] values)
        {
            if (values is not null && values.Length > 0)
            {
                result.ResultData.Add(errorCode, values.ToList());
            }
            return result;
        }
    }
}