using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class WarehouseRollbackConfiguration : IEntityTypeConfiguration<WarehouseRollback>
    {
        public void Configure(EntityTypeBuilder<WarehouseRollback> builder)
        {
            builder.ToTable("warehouserollback");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.OrderID).HasColumnName("OrderID").IsRequired();
            builder.Property(x => x.WarehouseID).HasColumnName("WarehouseID").IsRequired();
            builder.Property(x => x.QuantitySubtract).HasColumnName("QuantitySubtract").IsRequired();

            // Relationships
            builder.HasOne<Order>(wr => wr.Order)
                .WithMany()
                .HasForeignKey(wr => wr.OrderID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Warehouse>(wr => wr.Warehouse)
                .WithMany()
                .HasForeignKey(wr => wr.WarehouseID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
