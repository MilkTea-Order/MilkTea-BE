using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Inventory.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Inventory;

public class WarehouseRollbackConfiguration : IEntityTypeConfiguration<WarehouseRollbackEntity>
{
    public void Configure(EntityTypeBuilder<WarehouseRollbackEntity> builder)
    {
        builder.ToTable("WarehouseRollback");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID").ValueGeneratedOnAdd();
        builder.Property(x => x.WarehouseID).HasColumnName("WarehouseID").IsRequired();
        builder.Property(x => x.OrderID).HasColumnName("OrderID");
        builder.Property(x => x.QuantitySubtract).HasColumnName("QuantitySubtract").IsRequired();
    }
}
