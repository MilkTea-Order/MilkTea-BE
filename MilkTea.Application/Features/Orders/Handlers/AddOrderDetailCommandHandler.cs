using MilkTea.Application.Features.Catalog.Services;
using MilkTea.Application.Features.Orders.Commands;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.Orders.ValueObjects;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Handlers
{
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

            // Check order exist
            var order = await _vOrderUnitOfWork.Orders.GetOrderByIdWithItemsAsync(command.OrderID);
            if (order is null)
                return SendError(result, ErrorCode.E0001, "OrderID");

            // Check quantity
            foreach (var item in command.Items)
            {
                if (item.Quantity <= 0)
                {
                    SendError(result, ErrorCode.E0036, "Quantity");
                    AddItemMeta(result, item);
                    return result;
                }
            }

            var pairs = command.Items
                .Select(i => (i.MenuID, i.SizeID))
                .Distinct()
                .ToList();

            var canPayMap = await _vCatalogService.CanPayBatch(pairs, cancellationToken);

            foreach (var item in command.Items)
            {
                var key = (item.MenuID, item.SizeID);
                if (!canPayMap.TryGetValue(key, out var info) || !info.CanPay)
                {
                    SendError(result, ErrorCode.E0001, "Menu");
                    AddItemMeta(result, item);
                    return result;
                }
            }


            await _vOrderUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                foreach (var item in command.Items)
                {
                    var key = (item.MenuID, item.SizeID);
                    var info = canPayMap[key];

                    var (priceListId, price) = info.Data;

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
            result.ResultData.AddMeta("menuId", item.MenuID);
            result.ResultData.AddMeta("sizeId", item.SizeID);
        }

        private static AddOrderDetailResult SendError(AddOrderDetailResult result, string errorCode, params string[] values)
        {
            if (values is { Length: > 0 })
                result.ResultData.Add(errorCode, values.ToList());
            return result;
        }
    }
}
