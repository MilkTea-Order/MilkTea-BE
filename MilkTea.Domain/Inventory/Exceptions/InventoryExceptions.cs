using MilkTea.Domain.SharedKernel.Exceptions;

namespace MilkTea.Domain.Inventory.Exceptions
{
    public class InventoryMaterialRequiredExceptions : DomainException
    {
        public InventoryMaterialRequiredExceptions() : base("Inventory material exception occurred.") { }
    }

    public class InventoryMaterialPriceExceptions : DomainException
    {
        public InventoryMaterialPriceExceptions() : base("Inventory material price exception occurred.") { }
    }

    public class InventoryMaterialQuantityExceptions : DomainException
    {
        public InventoryMaterialQuantityExceptions() : base("Inventory material quantity exception occurred.") { }
    }

    public class InventorySupplierRequiredExceptions : DomainException
    {
        public InventorySupplierRequiredExceptions() : base("Inventory supplier required exception occurred.") { }
    }

    public class InventoryNotEnoughStockExceptions : DomainException
    {
        public IReadOnlyList<InventoryStockShortage> Shortages { get; }
        public InventoryNotEnoughStockExceptions(IReadOnlyList<InventoryStockShortage> shortages) : base("Inventory not enough stock exception occurred.")
        {
            Shortages = shortages;
        }
        public sealed class InventoryStockShortage
        {
            public int MaterialId { get; init; }
            public string MaterialName { get; init; } = default!;
            public decimal RequiredQuantity { get; init; }
            public decimal AvailableQuantity { get; init; }
        }
    }

}
