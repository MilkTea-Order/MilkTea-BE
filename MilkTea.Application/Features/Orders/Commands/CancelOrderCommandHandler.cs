using FluentValidation;
using MediatR;
using MilkTea.Application.Features.Orders.Models.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Common.Constants;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;

namespace MilkTea.Application.Features.Orders.Commands;


public class CancelOrderCommand : IRequest<CancelOrderResult>
{
    public int OrderId { get; init; }
}

public sealed class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName(nameof(CancelOrderCommand.OrderId));
    }
}

public sealed class CancelOrderCommandHandler(
    IOrderUnitOfWork orderingUnitOfWork,
    IIdentifyServicePorts currentUser) : IRequestHandler<CancelOrderCommand, CancelOrderResult>
{
    private readonly IOrderUnitOfWork _vOrderingUnitOfWork = orderingUnitOfWork;
    public async Task<CancelOrderResult> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        var result = new CancelOrderResult();

        var order = await _vOrderingUnitOfWork.Orders.GetOrderByIdWithItemsAsync(command.OrderId);
        if (order is null)
        {
            return SendError(result, ErrorCode.E0001, nameof(command.OrderId));
        }

        await _vOrderingUnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var cancelledBy = currentUser.UserId;
            order.CancelOrder(cancelledBy);
            await _vOrderingUnitOfWork.Orders.UpdateAsync(order);
            await _vOrderingUnitOfWork.CommitTransactionAsync(cancellationToken);
            return result;
        }
        catch (OrderNotEditableException)
        {
            await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E0042,  nameof(command.OrderId));
        }
        catch (OrderItemStatusInValidException)
        {
            await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E0042, "OrderItemInvalidStatusToCancelOrder");
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
