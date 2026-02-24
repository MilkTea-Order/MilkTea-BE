using FluentValidation;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands;


public class CancelOrderDetailsCommand : ICommand<CancelOrderDetailsResult>
{
    public int OrderID { get; set; }
    public List<int> OrderDetailIDs { get; set; } = new();
}

public sealed class CancelOrderDetailsCommandValidator : AbstractValidator<CancelOrderDetailsCommand>
{
    public CancelOrderDetailsCommandValidator()
    {
        // Check null, empty, and less than or equal to 0
        RuleFor(x => x.OrderID)
            .GreaterThan(0)
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName("OrderID");

        // check null and not empty and all greater than 0
        RuleFor(x => x.OrderDetailIDs)
            .NotNull()
            .NotEmpty()
            .Must(x => x.All(id => id > 0))
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName("OrderDetailIDs");

    }
}
public sealed class CancelOrderDetailsCommandHandler(
                                IOrderingUnitOfWork orderingUnitOfWork,
                                ICurrentUser currentUser) : ICommandHandler<CancelOrderDetailsCommand, CancelOrderDetailsResult>
{
    private readonly IOrderingUnitOfWork _vOrderingUnitOfWork = orderingUnitOfWork;
    public async Task<CancelOrderDetailsResult> Handle(CancelOrderDetailsCommand command, CancellationToken cancellationToken)
    {
        var result = new CancelOrderDetailsResult();

        // Load order with items for update
        var order = await _vOrderingUnitOfWork.Orders.GetOrderByIdWithItemsAsync(command.OrderID);
        if (order is null)
        {
            return SendError(result, ErrorCode.E0001, "OrderID");
        }

        // Cancel item not already cancelled
        await _vOrderingUnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var cancelledBy = currentUser.UserId;
            order.CancelOrderItems(command.OrderDetailIDs, cancelledBy);
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
            return SendError(result, ErrorCode.E0001, "OrderDetailIDs");
        }
        catch (OrderItemCancelledException)
        {
            await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E0042, "OrderDetailIDs");
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
