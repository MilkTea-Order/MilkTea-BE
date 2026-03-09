using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Inventory.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Inventory;

public class WarehouseConfiguration : IEntityTypeConfiguration<WarehouseEntity>
{
    public void Configure(EntityTypeBuilder<WarehouseEntity> builder)
    {
        builder.ToTable("Warehouse");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID").ValueGeneratedOnAdd();

        builder.Property(x => x.MaterialsID).HasColumnName("MaterialsID").IsRequired();
        builder.Property(x => x.QuantityImport).HasColumnName("QuantityImport").IsRequired();
        builder.Property(x => x.QuantityCurrent).HasColumnName("QuantityCurrent").IsRequired();
        builder.Property(x => x.PriceImport).HasColumnName("PriceImport").IsRequired();
        builder.Property(x => x.AmountTotal).HasColumnName("AmountTotal").IsRequired();
        builder.Property(x => x.ImportFromSuppliersID).HasColumnName("ImportFromSuppliersID").IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("StatusID")
            .HasConversion<int>()
            .IsRequired();

        builder.HasMany(x => x.WarehouseRollbacks)
                .WithOne()
                .HasForeignKey(x => x.WarehouseID)
                .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.WarehouseRollbacks)
                .HasField("_vWarehouseRollbacks")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(x => x.CreatedBy);
        builder.Ignore(x => x.CreatedDate);
        builder.Ignore(x => x.UpdatedBy);
        builder.Ignore(x => x.UpdatedDate);

    }
}
