using MediatR;
using MilkTea.Application.Features.Catalog.Abstractions.Constracts;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Configuration.Abstractions.Services;
using MilkTea.Domain.Inventory.Exceptions;
using MilkTea.Domain.Inventory.Repositories;
using MilkTea.Domain.Orders.Events;
using static MilkTea.Domain.Inventory.Exceptions.InventoryNotEnoughStockExceptions;

namespace MilkTea.Application.Features.Inventory.EventHandlers
{
    public class OrderCollectedDomainEventHandler(ICatalogService catalogService,
                                                    IInventoryUnitOfWork inventoryUnitOfWork,
                                                    IConfigurationService configurationService) : INotificationHandler<OrderCollectedDomainEvent>
    {
        private readonly ICatalogService _vCatalogService = catalogService;
        private readonly IInventoryUnitOfWork _vInventoryUnitOfWork = inventoryUnitOfWork;
        private readonly IConfigurationService _vConfigurationService = configurationService;
        public async Task Handle(OrderCollectedDomainEvent notification, CancellationToken cancellationToken)
        {
            if (!await _vConfigurationService.IsWarehouseManagementMode(cancellationToken)) return;
            var items = notification.Items.Select(x => new OrderItemsConstractDto
            {
                MenuId = x.MenuId,
                SizeId = x.SizeId,
                Quantity = x.Quantity
            }).ToList();

            var recipes = await _vCatalogService.GetMenuRecipesAsync(items, cancellationToken);

            var warehouses = await _vInventoryUnitOfWork.Warehouses.GetActiveByMaterialIdsAsync(recipes.Select(x => x.Id), cancellationToken);

            var warehouseMap = warehouses
                                        .GroupBy(x => x.MaterialsID)
                                        .ToDictionary(g => g.Key, g => g.OrderBy(x => x.Id).ToList());

            var errors = new List<InventoryStockShortage>();

            foreach (var need in recipes)
            {
                warehouseMap.TryGetValue(need.Id, out var materialWarehouses);

                var available = materialWarehouses?.Sum(x => x.QuantityCurrent) ?? 0;

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
                var materialWarehouses = warehouseMap[need.Id];
                var remaining = need.Quantity;

                foreach (var warehouse in materialWarehouses)
                {
                    if (remaining <= 0) break;

                    var deduct = Math.Min(warehouse.QuantityCurrent, remaining);

                    warehouse.DeductStock(notification.OrderId, deduct, need.Name);

                    remaining -= deduct;
                }
            }
        }
    }
}

//if (!await _vConfigurationService.IsWarehouseManagementMode(cancellationToken)) return;
//var items = notification.Items.Select(x => new OrderItemsConstractDto
//{
//    MenuId = x.MenuId,
//    SizeId = x.SizeId,
//    Quantity = x.Quantity
//}).ToList();

//var recipes = await _vCatalogService.GetMenuRecipesAsync(items, cancellationToken);

//var warehouses = await _vInventoryUnitOfWork.Warehouses.GetActiveByMaterialIdsAsync(recipes.Select(x => x.Id), cancellationToken);

//var warehouseMap = warehouses.ToDictionary(x => x.MaterialsID);

//var errors = new List<InventoryStockShortage>();

//foreach (var need in recipes)
//{
//    warehouseMap.TryGetValue(need.Id, out var warehouse);
//    var available = warehouse?.QuantityCurrent ?? 0;

//    if (available < need.Quantity)
//    {
//        errors.Add(new InventoryStockShortage
//        {
//            MaterialId = need.Id,
//            MaterialName = need.Name,
//            RequiredQuantity = need.Quantity,
//            AvailableQuantity = available
//        });
//    }
//}

//if (errors.Any()) throw new InventoryNotEnoughStockExceptions(errors);


//foreach (var need in recipes)
//{
//    var warehouse = warehouseMap[need.Id];
//    warehouse.DeductStock(notification.OrderId, need.Quantity, need.Name);
//}
//        }