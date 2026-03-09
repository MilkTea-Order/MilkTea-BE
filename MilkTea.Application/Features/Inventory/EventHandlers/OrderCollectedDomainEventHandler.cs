using MediatR;
using MilkTea.Application.Features.Catalog.Abstractions.Constracts;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Domain.Inventory.Exceptions;
using MilkTea.Domain.Inventory.Repositories;
using MilkTea.Domain.Orders.Events;
using static MilkTea.Domain.Inventory.Exceptions.InventoryNotEnoughStockExceptions;

namespace MilkTea.Application.Features.Inventory.EventHandlers
{
    public class OrderCollectedDomainEventHandler(ICatalogService catalogService,
                                                    IInventoryUnitOfWork inventoryUnitOfWork) : INotificationHandler<OrderCollectedDomainEvent>
    {
        private readonly ICatalogService _vCatalogService = catalogService;
        private readonly IInventoryUnitOfWork _vInventoryUnitOfWork = inventoryUnitOfWork;
        public async Task Handle(OrderCollectedDomainEvent notification, CancellationToken cancellationToken)
        {
            var items = notification.Items.Select(x => new OrderItemsConstractDto
            {
                MenuId = x.MenuId,
                SizeId = x.SizeId,
                Quantity = x.Quantity
            }).ToList();

            var recipes = await _vCatalogService.GetMenuRecipesAsync(items, cancellationToken);

            var warehouses = await _vInventoryUnitOfWork.Warehouses.GetActiveByMaterialIdsAsync(recipes.Select(x => x.Id), cancellationToken);

            var warehouseMap = warehouses.ToDictionary(x => x.MaterialsID);

            var errors = new List<InventoryStockShortage>();

            foreach (var need in recipes)
            {
                warehouseMap.TryGetValue(need.Id, out var warehouse);
                var available = warehouse?.QuantityCurrent ?? 0;

                if (available < need.Quantity)
                {
                    errors.Add(new InventoryStockShortage
                    {
                        MaterialId = need.Id,
                        MaterialName = need.Name,
                        RequiredQuantity = need.Quantity,
                        AvailableQuantity = available
                    });
                }
            }

            if (errors.Any()) throw new InventoryNotEnoughStockExceptions(errors);


            foreach (var need in recipes)
            {
                var warehouse = warehouseMap[need.Id];
                warehouse.DeductStock(notification.OrderId, need.Quantity, need.Name);
            }
        }
    }
}

//await _vInventoryUnitOfWork.BeginTransactionAsync(cancellationToken);
//try
//{
//    foreach (var need in recipes)
//    {
//        var warehouse = warehouseMap[need.Id];
//        warehouse.DeductStock(notification.OrderId, need.Quantity, need.Name);
//    }

//    await _vInventoryUnitOfWork.CommitTransactionAsync(cancellationToken);
//    return;
//}
//catch
//{
//    await _vInventoryUnitOfWork.RollbackTransactionAsync(cancellationToken);
//    throw;
//}
