using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Inventory.Entities;
using MilkTea.Domain.SharedKernel.Enums;

namespace MilkTea.Infrastructure.Persistence.Configurations.Inventory;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.ToTable("warehouse");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.MaterialsID).HasColumnName("MaterialsID").IsRequired();
        builder.Property(x => x.QuantityImport).HasColumnName("QuantityImport").IsRequired();
        builder.Property(x => x.QuantityCurrent).HasColumnName("QuantityCurrent").IsRequired();
        builder.Property(x => x.PriceImport).HasColumnName("PriceImport").IsRequired();
        builder.Property(x => x.AmountTotal).HasColumnName("AmountTotal").IsRequired();
        builder.Property(x => x.ImportFromSuppliersID).HasColumnName("ImportFromSuppliersID").IsRequired();
        
        // Map enum to existing StatusID column
        builder.Property(x => x.Status)
            .HasColumnName("StatusID")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy");
        builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");

        // Relationships
        builder.HasOne(w => w.Material)
            .WithMany()
            .HasForeignKey(w => w.MaterialsID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
