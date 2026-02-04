using MilkTea.Application.Features.Orders.Commands;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Handlers;

public sealed class CancelOrderDetailsCommandHandler(
    IOrderingUnitOfWork orderingUnitOfWork,
    ICurrentUser currentUser) : ICommandHandler<CancelOrderDetailsCommand, CancelOrderDetailsResult>
{
    private readonly IOrderingUnitOfWork _vOrderingUnitOfWork = orderingUnitOfWork;
    public async Task<CancelOrderDetailsResult> Handle(CancelOrderDetailsCommand command, CancellationToken cancellationToken)
    {
        var result = new CancelOrderDetailsResult();

        // If no order detail IDs provided, return empty result
        if (command.OrderDetailIDs is null || command.OrderDetailIDs.Count == 0)
            return result;

        // Load order with items for update
        var order = await _vOrderingUnitOfWork.Orders.GetOrderByIdWithItemsAsync(command.OrderID);
        if (order is null)
        {
            return SendError(result, ErrorCode.E0001, "OrderID");
        }

        // Validate order detail IDs belong to this order
        var validOrderDetailIds = order.OrderItems.Select(oi => oi.Id).ToList();
        var invalidDetailIds = command.OrderDetailIDs
            .Where(id => !validOrderDetailIds.Contains(id))
            .ToList();

        // If any invalid IDs found, return error
        if (invalidDetailIds.Count > 0)
        {
            result = SendError(result, ErrorCode.E0001, "OrderDetailIDs");
            result.ResultData.AddMeta("InvalidIDs", invalidDetailIds);
            return result;
        }
        // Cancel item not already cancelled
        await _vOrderingUnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var cancelledBy = currentUser.UserId;
            result.CancelledDetailIDs = order.CancelOrderItems(command.OrderDetailIDs, cancelledBy);
            await _vOrderingUnitOfWork.CommitTransactionAsync(cancellationToken);
            return result;
        }
        catch (OrderNotEditableException)
        {
            await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E0042, "OrderID");
        }
        catch (Exception)
        {
            await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E9999, "CancelOrderDetails");
        }
    }

    private static CancelOrderDetailsResult SendError(CancelOrderDetailsResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
