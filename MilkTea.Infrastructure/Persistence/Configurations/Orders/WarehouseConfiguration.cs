using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Entities.Users;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("warehouse");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.MaterialsID).HasColumnName("MaterialsID").IsRequired();
            builder.Property(x => x.QuantityImport).HasColumnName("QuantityImport").IsRequired();
            builder.Property(x => x.QuantityCurrent).HasColumnName("QuantityCurrent").IsRequired();
            builder.Property(x => x.PriceImport).HasColumnName("PriceImport").IsRequired();
            builder.Property(x => x.AmountTotal).HasColumnName("AmountTotal").IsRequired();
            builder.Property(x => x.ImportFromSuppliersID).HasColumnName("ImportFromSuppliersID").IsRequired();
            builder.Property(x => x.StatusID).HasColumnName("StatusID").IsRequired();

            // Relationships
            builder.HasOne<Material>(w => w.Material)
                .WithMany()
                .HasForeignKey(w => w.MaterialsID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Status>(w => w.Status)
                .WithMany()
                .HasForeignKey(w => w.StatusID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
