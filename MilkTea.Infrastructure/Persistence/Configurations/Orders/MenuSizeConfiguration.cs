using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class MenuSizeConfiguration : IEntityTypeConfiguration<MenuSize>
    {
        public void Configure(EntityTypeBuilder<MenuSize> builder)
        {
            builder.ToTable("menu_size");

            builder.HasKey(x => new { x.MenuID, x.SizeID });
            builder.Property(x => x.MenuID).HasColumnName("MenuID");
            builder.Property(x => x.SizeID).HasColumnName("SizeID");

            builder.Property(x => x.CostPrice).HasColumnName("CostPrice");
            builder.Property(x => x.SalePrice).HasColumnName("SalePrice");

            // Relationships
            builder.HasOne<Menu>(ms => ms.Menu)
                .WithMany()
                .HasForeignKey(ms => ms.MenuID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Size>(ms => ms.Size)
                .WithMany()
                .HasForeignKey(ms => ms.SizeID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
