using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Catalog.Entities.Menu;

namespace MilkTea.Infrastructure.Persistence.Configurations.Catalog;

public class MenuSizeConfiguration : IEntityTypeConfiguration<MenuSizeEntity>
{
    public void Configure(EntityTypeBuilder<MenuSizeEntity> builder)
    {
        builder.ToTable("Menu_Size");

        builder.HasKey(x => new { x.MenuID, x.SizeID });

        builder.Property(x => x.MenuID).HasColumnName("MenuID");
        builder.Property(x => x.SizeID).HasColumnName("SizeID");
        builder.Property(x => x.CostPrice).HasColumnName("CostPrice");
        builder.Property(x => x.SalePrice).HasColumnName("SalePrice");
    }
}
