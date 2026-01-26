using MilkTea.Domain.SharedKernel.Abstractions;
using MilkTea.Domain.SharedKernel.Enums;

namespace MilkTea.Domain.Inventory.Entities;

/// <summary>
/// Warehouse inventory entity (Aggregate Root).
/// </summary>
public sealed class Warehouse : Aggregate<int>
{
    public int MaterialsID { get; private set; }
    public decimal QuantityImport { get; private set; }
    public decimal QuantityCurrent { get; private set; }
    public decimal PriceImport { get; private set; }
    public decimal AmountTotal { get; private set; }
    public int ImportFromSuppliersID { get; private set; }
    
    /// <summary>
    /// Warehouse status. Maps to StatusID column.
    /// </summary>
    public CommonStatus Status { get; private set; }

    // Navigation
    public Material? Material { get; private set; }

    // For EF Core
    private Warehouse() { }

    public static Warehouse Create(
        int materialsId,
        decimal quantityImport,
        decimal priceImport,
        int importFromSuppliersId,
        int createdBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(materialsId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantityImport);
        ArgumentOutOfRangeException.ThrowIfNegative(priceImport);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(importFromSuppliersId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        var now = DateTime.UtcNow;
        var amountTotal = quantityImport * priceImport;

        return new Warehouse
        {
            MaterialsID = materialsId,
            QuantityImport = quantityImport,
            QuantityCurrent = quantityImport,
            PriceImport = priceImport,
            AmountTotal = amountTotal,
            ImportFromSuppliersID = importFromSuppliersId,
            Status = CommonStatus.Active,
            CreatedBy = createdBy,
            CreatedDate = now
        };
    }

    public bool IsActive => Status == CommonStatus.Active;

    public void DeductStock(decimal quantity, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        if (quantity > QuantityCurrent)
            throw new InvalidOperationException($"Insufficient stock. Current: {QuantityCurrent}, Requested: {quantity}");

        QuantityCurrent -= quantity;
        RecalculateAmountTotal();
        Touch(updatedBy);
    }

    public void AddStock(decimal quantity, decimal priceImport, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegative(priceImport);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        QuantityImport += quantity;
        QuantityCurrent += quantity;
        
        // Recalculate average price
        var totalValue = (QuantityImport - quantity) * PriceImport + quantity * priceImport;
        PriceImport = QuantityImport > 0 ? totalValue / QuantityImport : priceImport;
        
        RecalculateAmountTotal();
        Touch(updatedBy);
    }

    public void Activate(int activatedBy)
    {
        if (Status == CommonStatus.Active)
            throw new InvalidOperationException("Warehouse is already active.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(activatedBy);

        Status = CommonStatus.Active;
        Touch(activatedBy);
    }

    public void Deactivate(int deactivatedBy)
    {
        if (Status == CommonStatus.Inactive)
            throw new InvalidOperationException("Warehouse is already inactive.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(deactivatedBy);

        Status = CommonStatus.Inactive;
        Touch(deactivatedBy);
    }

    private void RecalculateAmountTotal()
    {
        AmountTotal = QuantityCurrent * PriceImport;
    }

    private void Touch(int updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }
}
