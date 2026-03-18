using MilkTea.Domain.Inventory.Enums;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Inventory.Entities
{
    public class ImportFromSupplierEntity : Aggregate<int>
    {
        public int SupplierId { get; private set; }
        public string? BillNo { get; private set; }
        public DateTime? BillDate { get; private set; }

        public DateTime ImportedDate { get; private set; }
        public int ImportedBy { get; private set; }

        public string? Note { get; private set; }
        public InventoryStatus Status { get; private set; }

        public int? ApprovedBy { get; private set; }
        public DateTime? ApprovedDate { get; private set; }
        private ImportFromSupplierEntity() { }
    }
}
