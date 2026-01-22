using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class MenuAndMaterialConfiguration : IEntityTypeConfiguration<MenuAndMaterial>
    {
        public void Configure(EntityTypeBuilder<MenuAndMaterial> builder)
        {
            builder.ToTable("menuandmaterials");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.MenuID).HasColumnName("MenuID").IsRequired();
            builder.Property(x => x.SizeID).HasColumnName("SizeID").IsRequired();
            builder.Property(x => x.MaterialsID).HasColumnName("MaterialsID").IsRequired();
            builder.Property(x => x.Quantity).HasColumnName("Quantity").IsRequired();

            // Relationships
            builder.HasOne<Menu>(mm => mm.Menu)
                .WithMany()
                .HasForeignKey(mm => mm.MenuID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Material>(mm => mm.Material)
                .WithMany()
                .HasForeignKey(mm => mm.MaterialsID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Size>(mm => mm.Size)
                .WithMany()
                .HasForeignKey(mm => mm.SizeID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
