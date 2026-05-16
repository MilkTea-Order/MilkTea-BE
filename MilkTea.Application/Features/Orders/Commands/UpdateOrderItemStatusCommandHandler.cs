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
    public List<UpdateOrderItemStatusItem> Items { get; set; } = [];
}

public class UpdateOrderItemStatusItem
{
    public int OrderId { get; set; }
    public int OrderDetailId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Reason { get; set; }
}

public sealed class UpdateOrderItemStatusCommandHandler(IOrderUnitOfWork orderingUnitOfWork,
                                                                    IIdentifyServicePorts currentUser) 
                                        : ICommandHandler<UpdateOrderItemStatusCommand, UpdateOrderItemStatusResult>
{
    private readonly IOrderUnitOfWork _vOrderingUnitOfWork = orderingUnitOfWork;

    public async Task<UpdateOrderItemStatusResult> Handle(UpdateOrderItemStatusCommand command, CancellationToken cancellationToken)
    {
        UpdateOrderItemStatusResult result = new();
        var failedItems = new Dictionary<string, (int OrderId, int OrderDetailId, string ErrorCode, string ErrorField)>();

        // Validate outside transaction
        foreach (var item in command.Items)
        {
            var key = $"{item.OrderId}:{item.OrderDetailId}";
            if (!OrderDetailStatusExtensions.TryParse(item.Status, out _))
            {
                failedItems.TryAdd(key, (item.OrderId, item.OrderDetailId, ErrorCode.E0001, nameof(item.Status)));
                continue;
            }

            OrderDetailStatusExtensions.TryParse(item.Status, out var newStatus);
            if (newStatus == OrderItemStatus.Cancelled && string.IsNullOrWhiteSpace(item.Reason))
            {
                failedItems.TryAdd(key, (item.OrderId, item.OrderDetailId, ErrorCode.E0036, nameof(item.Reason)));
            }
        }

        if (failedItems.Count != 0)
        {
            BuildErrorResult(result, failedItems.Values.ToList());
            return result;
        }

        await _vOrderingUnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            foreach (var item in command.Items)
            {
                var key = $"{item.OrderId}:{item.OrderDetailId}";
                var order = await _vOrderingUnitOfWork.Orders.GetOrderByIdWithItemsAsync(item.OrderId);
                if (order is null)
                {
                    failedItems.TryAdd(key, (item.OrderId, item.OrderDetailId, ErrorCode.E0001, nameof(item.OrderId)));
                    continue;
                }

                try
                {
                    OrderDetailStatusExtensions.TryParse(item.Status, out var newStatus);
                    order.UpdateOrderItemStatus(item.OrderDetailId, newStatus, currentUser.UserId, item.Reason);
                }
                catch (OrderNotEditableException)
                {
                    failedItems.TryAdd(key, (item.OrderId, item.OrderDetailId, ErrorCode.E0042, nameof(item.OrderId)));
                }
                catch (OrderItemNotFoundException)
                {
                    failedItems.TryAdd(key, (item.OrderId, item.OrderDetailId, ErrorCode.E0001, nameof(item.OrderDetailId)));
                }
                // catch (OrderItemCancelledException)
                // {
                //     failedItems.TryAdd(key, (item.OrderId, item.OrderDetailId, ErrorCode.E0042, nameof(item.OrderDetailId)));
                // }
                catch (InvalidOrderDetailStatusTransitionException)
                {
                    failedItems.TryAdd(key, (item.OrderId, item.OrderDetailId, ErrorCode.E0042, nameof(item.Status)));
                }
            }

            if (failedItems.Count != 0)
            {
                await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
                BuildErrorResult(result, failedItems.Values.ToList());
                return result;
            }

            await _vOrderingUnitOfWork.CommitTransactionAsync(cancellationToken);
            return result;
        }
        catch (Exception)
        {
            await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    private static void BuildErrorResult(UpdateOrderItemStatusResult result, List<(int OrderId, int OrderDetailId, string ErrorCode, string ErrorField)> failedItems)
    {
        var groupedByError = failedItems.GroupBy(f => f.ErrorCode);
        foreach (var group in groupedByError)
        {
            result.ResultData.Add(group.Key, group.Select(f => $"{f.OrderId}:{f.OrderDetailId}").ToList());
            AddItemMeta(result, group.Select(f => new
            {
                key= $"{f.OrderId}:{f.OrderDetailId}",
                OrderId = f.OrderId,
                OrderDetailId = f.OrderDetailId,
                ErrorCode = f.ErrorCode,
                ErrorField = f.ErrorField
            }), group.Key);
        }
    }

    private static void AddItemMeta(UpdateOrderItemStatusResult result, dynamic items, string key)
    {
        result.ResultData.AddMeta(key, items);
    }
}
