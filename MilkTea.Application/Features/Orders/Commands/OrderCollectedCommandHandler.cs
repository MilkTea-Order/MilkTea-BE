using FluentValidation;
using MilkTea.Application.Features.Orders.Models.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Inventory.Exceptions;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Shared.Domain.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands
{
    public class OrderCollectedCommand : ICommand<OrderCollectedCommandResult>
    {
        public int OrderId { get; set; }
    }

    public class OrderCollectedCommandValidator : AbstractValidator<OrderCollectedCommand>
    {
        public OrderCollectedCommandValidator()
        {
            RuleFor(x => x.OrderId).GreaterThan(0)
                                    .WithErrorCode(ErrorCode.E0001)
                                    .OverridePropertyName(nameof(OrderCollectedCommand.OrderId));
        }
    }

    public class OrderCollectedCommandHandler(IOrderUnitOfWork orderUnitOfWork,
                                                ICurrentUser currentUser) : ICommandHandler<OrderCollectedCommand, OrderCollectedCommandResult>
    {
        private readonly IOrderUnitOfWork _vOrderUnitOfWork = orderUnitOfWork;
        private readonly ICurrentUser _vCurrentUser = currentUser;

        public async Task<OrderCollectedCommandResult> Handle(OrderCollectedCommand command, CancellationToken cancellationToken)
        {
            OrderCollectedCommandResult result = new();
            var order = await _vOrderUnitOfWork.Orders.GetOrderByIdWithItemsAsync(command.OrderId);
            if (order is null) return SendError(result, ErrorCode.E0001, nameof(command.OrderId));

            await _vOrderUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                order.MaskAsCollected(_vCurrentUser.UserId);
                order.FinalizeAndPublishCollected();
                await _vOrderUnitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch (NotExistActionBy)
            {
                await _vOrderUnitOfWork.RollbackTransactionAsync(cancellationToken);
                result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
                return result;
            }
            catch (OrderNotEditableException)
            {
                await _vOrderUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0042, nameof(command.OrderId));
            }
            catch (InventoryNotEnoughStockExceptions ex)
            {
                await _vOrderUnitOfWork.RollbackTransactionAsync(cancellationToken);
                result = SendError(result, ErrorCode.E0041, nameof(command.OrderId));
                AddItemMeta(result, ex.Shortages.Select(s => new
                {
                    s.MaterialId,
                    s.MaterialName,
                    s.RequiredQuantity,
                    s.AvailableQuantity
                }), ErrorCode.E0041);
                return result;
            }
            catch (Exception)
            {
                await _vOrderUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E9999, "OrderCollected");
            }
            return result;
        }

        private static void AddItemMeta(OrderCollectedCommandResult result, dynamic item, string key)
        {
            result.ResultData.AddMeta(key, item);
        }

        private static OrderCollectedCommandResult SendError(OrderCollectedCommandResult result, string errorCode, params string[] values)
        {
            if (values is { Length: > 0 })
            {
                result.ResultData.Add(errorCode, values.ToList());
            }
            return result;
        }
    }
}
