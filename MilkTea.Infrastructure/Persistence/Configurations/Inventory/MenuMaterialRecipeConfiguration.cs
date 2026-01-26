using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Inventory.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Inventory;

public class MenuMaterialRecipeConfiguration : IEntityTypeConfiguration<MenuMaterialRecipe>
{
    public void Configure(EntityTypeBuilder<MenuMaterialRecipe> builder)
    {
        builder.ToTable("menuandmaterial");

        builder.HasKey(x => new { x.MenuID, x.MaterialID });

        builder.Property(x => x.MenuID).HasColumnName("MenuID");
        builder.Property(x => x.MaterialID).HasColumnName("MaterialID");
        builder.Property(x => x.Quantity).HasColumnName("Quantity").IsRequired();
        builder.Property(x => x.UnitID).HasColumnName("UnitID");

        // Relationships
        builder.HasOne(r => r.Material)
            .WithMany()
            .HasForeignKey(r => r.MaterialID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Unit)
            .WithMany()
            .HasForeignKey(r => r.UnitID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
