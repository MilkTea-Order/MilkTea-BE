using MediatR;
using MilkTea.Application.Features.Orders.Commands;
using MilkTea.Application.Ports.Users;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.SharedKernel.Constants;

namespace MilkTea.Application.Features.Orders.Commands;

public sealed class CancelOrderDetailsCommandHandler(
    IOrderingUnitOfWork orderingUnitOfWork,
    ICurrentUser currentUser) : IRequestHandler<CancelOrderDetailsCommand, CancelOrderDetailsResult>
{
    public async Task<CancelOrderDetailsResult> Handle(CancelOrderDetailsCommand command, CancellationToken cancellationToken)
    {
        var result = new CancelOrderDetailsResult();

        // If no order detail IDs provided, return empty result
        if (command.OrderDetailIDs is null || command.OrderDetailIDs.Count == 0)
            return result;

        // Load order with items for update
        var order = await orderingUnitOfWork.Orders.GetOrderByIdWithItemsAsync(command.OrderID);
        if (order is null)
            return SendError(result, ErrorCode.E0001, nameof(command.OrderID));

        // Check if order can have items cancelled (must be Unpaid)
        if (order.Status != Domain.Orders.Enums.OrderStatus.Unpaid)
            return SendError(result, ErrorCode.E0042, "Order");

        // Validate order detail IDs belong to this order
        var validOrderDetailIds = order.OrderItems.Select(oi => oi.Id).ToList();
        var invalidDetailIds = command.OrderDetailIDs
            .Where(id => !validOrderDetailIds.Contains(id))
            .ToList();

        if (invalidDetailIds.Count > 0)
            return SendError(result, ErrorCode.E0001, "OrderDetailIDs");

        // Check if any order detail is already cancelled
        var alreadyCancelledIds = command.OrderDetailIDs
            .Where(id => order.OrderItems.FirstOrDefault(oi => oi.Id == id)?.IsCancelled == true)
            .ToList();

        if (alreadyCancelledIds.Count > 0)
            return SendError(result, ErrorCode.E0029, $"OrderDetailIDs: {string.Join(", ", alreadyCancelledIds)}");

        await orderingUnitOfWork.BeginTransactionAsync();
        try
        {
            var cancelledBy = currentUser.UserId;

            // Use domain method to cancel multiple order items
            order.RemoveOrderItems(command.OrderDetailIDs, cancelledBy);

            await orderingUnitOfWork.Orders.UpdateAsync(order);
            await orderingUnitOfWork.CommitTransactionAsync();

            var successfullyCancelledIds = command.OrderDetailIDs
                .Where(id => order.OrderItems.FirstOrDefault(oi => oi.Id == id)?.IsCancelled == true)
                .ToList();

            result.OrderID = order.Id;
            result.CancelledDetailIDs = successfullyCancelledIds;
            result.CancelledDate = DateTime.UtcNow;

            return result;
        }
        catch (Exception)
        {
            await orderingUnitOfWork.RollbackTransactionAsync();
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
