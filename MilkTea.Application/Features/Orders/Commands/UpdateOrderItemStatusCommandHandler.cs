using FluentValidation;
using MilkTea.Application.Features.Orders.Models.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Common.Constants;
using MilkTea.Domain.Orders.Enums;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands;

public class UpdateOrderItemStatusCommand : ICommand<UpdateOrderItemStatusResult>
{
    public int OrderID { get; set; }
    public int OrderDetailID { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Reason { get; set; }
}

    public sealed class UpdateOrderItemStatusCommandValidator : AbstractValidator<UpdateOrderItemStatusCommand>
    {
        public UpdateOrderItemStatusCommandValidator()
        {
            RuleFor(x => x.OrderID)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName(nameof(UpdateOrderDetailCommand.OrderID));

            RuleFor(x => x.OrderDetailID)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName(nameof(UpdateOrderDetailCommand.OrderDetailID));

            RuleFor(x => x.Status)
                .Must(statusName => OrderDetailStatusExtensions.TryParse(statusName, out _))
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName(nameof(UpdateOrderItemStatusCommand.Status));

            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName(nameof(UpdateOrderItemStatusCommand.Reason))
                .When(x => OrderDetailStatusExtensions.TryParse(x.Status, out var s) && s == OrderDetailStatus.Cancelled);
        }
    }

public sealed class UpdateOrderItemStatusCommandHandler(
    IOrderUnitOfWork orderingUnitOfWork,
    IIdentifyServicePorts currentUser) : ICommandHandler<UpdateOrderItemStatusCommand, UpdateOrderItemStatusResult>
{
    private readonly IOrderUnitOfWork _vOrderingUnitOfWork = orderingUnitOfWork;

    public async Task<UpdateOrderItemStatusResult> Handle(UpdateOrderItemStatusCommand command, CancellationToken cancellationToken)
    {
        var result = new UpdateOrderItemStatusResult();

        var newStatus = Enum.Parse<OrderDetailStatus>(command.Status, ignoreCase: true);

        var order = await _vOrderingUnitOfWork.Orders.GetOrderByIdWithItemsAsync(command.OrderID);
        if (order is null)
        {
            return SendError(result, ErrorCode.E0001, nameof(command.OrderID));
        }

        await _vOrderingUnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            order.UpdateOrderItemStatus(command.OrderDetailID, newStatus, currentUser.UserId, command.Reason);
            await _vOrderingUnitOfWork.CommitTransactionAsync(cancellationToken);
            return result;
        }
        catch (OrderNotEditableException)
        {
            await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E0042, nameof(command.OrderID));
        }
        catch (OrderItemNotFoundException)
        {
            await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E0001, nameof(command.OrderDetailID));
        }
        catch (OrderItemCancelledException)
        {
            await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E0042, nameof(command.OrderDetailID));
        }
        catch (InvalidOrderDetailStatusTransitionException)
        {
            await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E0042, nameof(command.Status));
        }
        catch (Exception)
        {
            await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E9999, "UpdateOrderItemStatus");
        }
    }

    private static UpdateOrderItemStatusResult SendError(UpdateOrderItemStatusResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
