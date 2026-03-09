using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Inventory.Entities;

public class WarehouseRollbackEntity : EntityId<int>
{
    public int WarehouseID { get; private set; }
    public int OrderID { get; private set; }
    public decimal QuantitySubtract { get; private set; }
    private WarehouseRollbackEntity() { }

    public static WarehouseRollbackEntity Create(int warehouseId, int orderId, decimal quantitySubtract)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(warehouseId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(orderId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantitySubtract);
        return new WarehouseRollbackEntity
        {
            WarehouseID = warehouseId,
            OrderID = orderId,
            QuantitySubtract = quantitySubtract
        };
    }
}