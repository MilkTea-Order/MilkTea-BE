using FluentValidation;
using MilkTea.Application.Features.Catalog.Abstractions;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.Orders.ValueObjects;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands
{
    public class AddOrderDetailCommand : ICommand<AddOrderDetailResult>
    {
        public int OrderID { get; set; }
        public List<OrderItemCommand> Items { get; set; } = new();
    }

    public sealed class AddOrderDetailCommandValidator : AbstractValidator<AddOrderDetailCommand>
    {
        public AddOrderDetailCommandValidator()
        {
            RuleFor(x => x.OrderID)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName("orderID");

            RuleFor(x => x.Items)
                .NotNull()
                .NotEmpty()
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName("items");
        }
    }

    public class AddOrderDetailCommandHandler(
        IOrderingUnitOfWork orderingUnitOfWork,
        ICatalogService catalogSalesService,
        ICurrentUser currentUser
    ) : ICommandHandler<AddOrderDetailCommand, AddOrderDetailResult>
    {
        private readonly IOrderingUnitOfWork _vOrderUnitOfWork = orderingUnitOfWork;
        private readonly ICatalogService _vCatalogService = catalogSalesService;
        private readonly ICurrentUser _vCurrentUser = currentUser;

        public async Task<AddOrderDetailResult> Handle(AddOrderDetailCommand command, CancellationToken cancellationToken)
        {
            var result = new AddOrderDetailResult();
            var createdBy = _vCurrentUser.UserId;
            var hasError = false;

            // Check order exist
            var order = await _vOrderUnitOfWork.Orders.GetOrderByIdWithItemsAsync(command.OrderID);
            if (order is null)
            {
                return SendError(result, ErrorCode.E0001, "OrderID");
            }

            // Check quantity
            foreach (var item in command.Items)
            {
                if (item.Quantity <= 0)
                {
                    if (!hasError)
                    {
                        result = SendError(result, ErrorCode.E0036, "quantity");
                        hasError = true;
                    }
                    AddItemMeta(result, item);
                }
            }
            if (hasError) return result;

            // Check menu can pay
            var pairs = command.Items.Select(i => (i.MenuID, i.SizeID)).Distinct().ToList();
            var canPayMap = await _vCatalogService.CanPayBatch(pairs, cancellationToken);
            foreach (var item in command.Items)
            {
                var key = (item.MenuID, item.SizeID);
                if (!canPayMap.TryGetValue(key, out var info) || !info.CanPay)
                {
                    if (!hasError)
                    {
                        result = SendError(result, ErrorCode.E0001, "Menu");
                        hasError = true;
                    }
                    AddItemMeta(result, item);
                }
            }
            if (hasError) return result;

            await _vOrderUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                foreach (var item in command.Items)
                {
                    var key = (item.MenuID, item.SizeID);
                    var info = canPayMap[key];

                    var (priceListId, price) = info.Data;

                    // If the item already exists in the order, update quantity instead of adding a new one
                    //var exists = order.OrderItems.Any(od => od.MenuItem.MenuId == item.MenuID
                    //                            && od.MenuItem.SizeId == item.SizeID
                    //                            && od.MenuItem.Price == price
                    //                            && od.MenuItem.PriceListId == priceListId);

                    var menuItem = MenuItem.Of(
                        menuId: item.MenuID,
                        sizeId: item.SizeID,
                        price: price,
                        priceListId: priceListId,
                        kindOfHotpot1Id: item.KindOfHotpotIDs?.Count > 0 ? item.KindOfHotpotIDs[0] : null,
                        kindOfHotpot2Id: item.KindOfHotpotIDs?.Count > 1 ? item.KindOfHotpotIDs[1] : null);

                    order.CreateOrderItem(
                        menuItem: menuItem,
                        quantity: item.Quantity,
                        createdBy: createdBy,
                        note: item.Note);
                }
                await _vOrderUnitOfWork.CommitTransactionAsync(cancellationToken);
                return result;
            }
            catch (OrderNotEditableException)
            {
                await _vOrderUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0042, "OrderID");
            }
            catch (Exception)
            {
                await _vOrderUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E9999, "AddOrderDetail");
            }
        }

        private static void AddItemMeta(AddOrderDetailResult result, dynamic item)
        {
            var i = new
            {
                menuId = item.MenuID,
                sizeId = item.SizeID
            };
            result.ResultData.AddMeta("InvalidItems", i);
        }

        private static AddOrderDetailResult SendError(AddOrderDetailResult result, string errorCode, params string[] values)
        {
            if (values is { Length: > 0 })
                result.ResultData.Add(errorCode, values.ToList());
            return result;
        }
    }
}
