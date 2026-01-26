using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Inventory.Entities;

/// <summary>
/// Warehouse rollback entity for tracking inventory changes.
/// </summary>
public class WarehouseRollback : Entity<int>
{
    public int WarehouseID { get; set; }
    public decimal QuantityBefore { get; set; }
    public decimal QuantityAfter { get; set; }
    public int? OrderID { get; set; }
    public int? OrderDetailID { get; set; }
    public string? Note { get; set; }

    // Navigation
    public Warehouse? Warehouse { get; set; }
}
