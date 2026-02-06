using MediatR;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.SharedKernel.Constants;

namespace MilkTea.Application.Features.Orders.Commands;

public sealed class CancelOrderCommandHandler(
    IOrderingUnitOfWork orderingUnitOfWork,
    ICurrentUser currentUser) : IRequestHandler<CancelOrderCommand, CancelOrderResult>
{
    private readonly IOrderingUnitOfWork _vOrderingUnitOfWork = orderingUnitOfWork;
    public async Task<CancelOrderResult> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        var result = new CancelOrderResult();

        var order = await _vOrderingUnitOfWork.Orders.GetOrderByIdWithItemsAsync(command.OrderID);
        if (order is null)
        {
            return SendError(result, ErrorCode.E0001, nameof(command.OrderID));
        }

        await _vOrderingUnitOfWork.BeginTransactionAsync();
        try
        {
            var cancelledBy = currentUser.UserId;
            order.Cancel(cancelledBy);
            await _vOrderingUnitOfWork.Orders.UpdateAsync(order);
            await _vOrderingUnitOfWork.CommitTransactionAsync();
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
            return SendError(result, ErrorCode.E9999, "CancelOrder");
        }
    }

    private static CancelOrderResult SendError(CancelOrderResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
