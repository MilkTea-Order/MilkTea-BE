using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Inventory.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Inventory;

public class WarehouseRollbackConfiguration : IEntityTypeConfiguration<WarehouseRollback>
{
    public void Configure(EntityTypeBuilder<WarehouseRollback> builder)
    {
        builder.ToTable("WarehouseRollback");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");

        builder.Property(x => x.WarehouseID).HasColumnName("WarehouseID").IsRequired();
        builder.Property(x => x.QuantityBefore).HasColumnName("QuantityBefore").IsRequired();
        builder.Property(x => x.QuantityAfter).HasColumnName("QuantityAfter").IsRequired();
        builder.Property(x => x.OrderID).HasColumnName("OrderID");
        builder.Property(x => x.OrderDetailID).HasColumnName("OrderDetailID");
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.Note).HasColumnName("Note");

        // Relationships
        builder.HasOne(wr => wr.Warehouse)
            .WithMany()
            .HasForeignKey(wr => wr.WarehouseID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
