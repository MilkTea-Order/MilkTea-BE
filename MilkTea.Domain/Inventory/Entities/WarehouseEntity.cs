using MilkTea.Domain.Inventory.Enums;
using MilkTea.Domain.Inventory.Exceptions;
using MilkTea.Domain.SharedKernel.Abstractions;
using static MilkTea.Domain.Inventory.Exceptions.InventoryNotEnoughStockExceptions;

namespace MilkTea.Domain.Inventory.Entities;

public sealed class WarehouseEntity : Aggregate<int>
{
    private readonly List<WarehouseRollbackEntity> _vWarehouseRollbacks = new();
    public IReadOnlyList<WarehouseRollbackEntity> WarehouseRollbacks => _vWarehouseRollbacks.AsReadOnly();

    public int MaterialsID { get; private set; }
    public decimal QuantityImport { get; private set; }
    public decimal QuantityCurrent { get; private set; }
    public decimal PriceImport { get; private set; }
    public decimal AmountTotal { get; private set; }
    public int ImportFromSuppliersID { get; private set; }
    public InventoryStatus Status { get; private set; }
    public bool IsActive => Status == InventoryStatus.InStock;

    private WarehouseEntity() { }

    public static WarehouseEntity Create(int materialsId,
                                        decimal quantityImport,
                                        decimal priceImport,
                                        int importFromSuppliersId)
    {
        if (materialsId <= 0)
        {
            throw new InventoryMaterialRequiredExceptions();
        }

        if (quantityImport <= 0)
        {
            throw new InventoryMaterialQuantityExceptions();
        }
        if (priceImport < 0)
        {
            throw new InventoryMaterialPriceExceptions();
        }
        if (importFromSuppliersId <= 0)
        {
            throw new InventorySupplierRequiredExceptions();
        }

        return new WarehouseEntity
        {
            MaterialsID = materialsId,
            QuantityImport = quantityImport,
            QuantityCurrent = quantityImport,
            PriceImport = priceImport,
            AmountTotal = quantityImport * priceImport,
            ImportFromSuppliersID = importFromSuppliersId,
            Status = InventoryStatus.InStock,
        };
    }

    public void DeductStock(int orderId, decimal quantity, string? name = "Không rõ")
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(orderId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        if (quantity > QuantityCurrent)
        {
            var shortage = new InventoryStockShortage
            {
                MaterialId = MaterialsID,
                MaterialName = name ?? "Không rõ",
                RequiredQuantity = quantity,
                AvailableQuantity = QuantityCurrent
            };
            throw new InventoryNotEnoughStockExceptions(new List<InventoryStockShortage> { shortage });
        }
        QuantityCurrent -= quantity;

        var rollback = WarehouseRollbackEntity.Create(Id, orderId, quantity);
        _vWarehouseRollbacks.Add(rollback);
    }
}
