using MediatR;
using MilkTea.Application.Features.Orders.Commands;
using MilkTea.Application.Ports.Users;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.SharedKernel.Repositories;

namespace MilkTea.Application.Features.Orders.Commands;

public sealed class CancelOrderCommandHandler(
    IUnitOfWork unitOfWork,
    IOrderRepository orderRepository,
    ICurrentUser currentUser) : IRequestHandler<CancelOrderCommand, CancelOrderResult>
{
    public async Task<CancelOrderResult> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        var result = new CancelOrderResult();

        // Get order
        var order = await orderRepository.GetOrderByIdAsync(command.OrderID);
        if (order is null)
            return SendError(result, ErrorCode.E0001, nameof(command.OrderID));

        // Check if order can be cancelled (must be Unpaid)
        if (order.Status != Domain.Orders.Enums.OrderStatus.Unpaid)
            return SendError(result, ErrorCode.E0042, nameof(command.OrderID));

        await unitOfWork.BeginTransactionAsync();
        try
        {
            var cancelledBy = currentUser.UserId;

            // Cancel order using domain method
            if (!string.IsNullOrWhiteSpace(command.CancelNote))
            {
                order.UpdateNote(command.CancelNote, cancelledBy);
            }

            order.Cancel(cancelledBy);

            await orderRepository.UpdateAsync(order);
            await unitOfWork.CommitTransactionAsync();

            result.OrderID = order.Id;
            result.BillNo = order.BillNo.Value;
            result.CancelledDate = order.CancelledDate ?? DateTime.UtcNow;

            return result;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync();
            return SendError(result, ErrorCode.E0027, "Cancel Order Request");
        }
    }

    private static CancelOrderResult SendError(CancelOrderResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
