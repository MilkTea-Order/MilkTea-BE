using MediatR;
using MilkTea.Application.Features.Orders.Commands;
using MilkTea.Application.Ports.Users;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.SharedKernel.Repositories;

namespace MilkTea.Application.Features.Orders.Commands;

public sealed class CancelOrderDetailsCommandHandler(
    IUnitOfWork unitOfWork,
    IOrderRepository orderRepository,
    ICurrentUser currentUser) : IRequestHandler<CancelOrderDetailsCommand, CancelOrderDetailsResult>
{
    public async Task<CancelOrderDetailsResult> Handle(CancelOrderDetailsCommand command, CancellationToken cancellationToken)
    {
        var result = new CancelOrderDetailsResult();

        // Validate order exists
        var order = await orderRepository.GetOrderByIdAsync(command.OrderID);
        if (order is null)
            return SendError(result, ErrorCode.E0001, nameof(command.OrderID));

        // Check if order can have items cancelled (must be Unpaid)
        if (order.Status != Domain.Orders.Enums.OrderStatus.Unpaid)
            return SendError(result, ErrorCode.E0042, "Order");

        // If no order detail IDs provided, return empty result
        if (command.OrderDetailIDs is null || command.OrderDetailIDs.Count == 0)
            return result;

        // Validate order detail IDs belong to this order
        var validOrderDetailIds = await orderRepository.GetOrderItemIdsByOrderIdAsync(command.OrderID);
        var invalidDetailIds = command.OrderDetailIDs
            .Where(id => !validOrderDetailIds.Contains(id))
            .ToList();

        if (invalidDetailIds.Count > 0)
            return SendError(result, ErrorCode.E0001, "OrderDetailIDs");

        // Check if any order detail is already cancelled
        var alreadyCancelledIds = new List<int>();
        foreach (var detailId in command.OrderDetailIDs)
        {
            var isCancelled = await orderRepository.IsOrderItemCancelledAsync(detailId);
            if (isCancelled)
            {
                alreadyCancelledIds.Add(detailId);
            }
        }

        if (alreadyCancelledIds.Count > 0)
            return SendError(result, ErrorCode.E0029, $"OrderDetailIDs: {string.Join(", ", alreadyCancelledIds)}");

        await unitOfWork.BeginTransactionAsync();
        try
        {
            var cancelledBy = currentUser.UserId;
            var successfullyCancelledIds = new List<int>();

            foreach (var detailId in command.OrderDetailIDs)
            {
                // Use domain method to cancel order item
                order.RemoveOrderItem(detailId, cancelledBy);
                successfullyCancelledIds.Add(detailId);
            }

            if (successfullyCancelledIds.Count == 0)
            {
                await unitOfWork.RollbackTransactionAsync();
                return SendError(result, ErrorCode.E0027, "Cancel Order Details");
            }

            await orderRepository.UpdateAsync(order);
            await unitOfWork.CommitTransactionAsync();

            result.OrderID = order.Id;
            result.CancelledDetailIDs = successfullyCancelledIds;
            result.CancelledDate = DateTime.UtcNow;

            return result;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync();
            return SendError(result, ErrorCode.E0027, "Cancel Order Details");
        }
    }

    private static CancelOrderDetailsResult SendError(CancelOrderDetailsResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
