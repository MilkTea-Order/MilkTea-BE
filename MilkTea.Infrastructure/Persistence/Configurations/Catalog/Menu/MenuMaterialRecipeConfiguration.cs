using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Catalog.Menu.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Catalog.Menu;

public class MenuMaterialRecipeConfiguration : IEntityTypeConfiguration<MenuMaterialRecipeEntity>
{
    public void Configure(EntityTypeBuilder<MenuMaterialRecipeEntity> builder)
    {
        builder.ToTable("MenuAndMaterials");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID").ValueGeneratedOnAdd();

        builder.Property(x => x.MenuID).HasColumnName("MenuID").IsRequired();
        builder.Property(x => x.SizeID).HasColumnName("SizeID").IsRequired();
        builder.Property(x => x.MaterialID).HasColumnName("MaterialsID").IsRequired();
        builder.Property(x => x.Quantity).HasColumnName("Quantity").IsRequired();
    }
}
