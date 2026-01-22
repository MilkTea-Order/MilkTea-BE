using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class MaterialConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.ToTable("materials");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
            builder.Property(x => x.Code).HasColumnName("Code");
            builder.Property(x => x.UnitID).HasColumnName("UnitID");
            builder.Property(x => x.UnitID_Max).HasColumnName("UnitID_Max");
            builder.Property(x => x.StyleQuantity).HasColumnName("StyleQuantity");
            builder.Property(x => x.MaterialsGroupID).HasColumnName("MaterialsGroupID").IsRequired();
            builder.Property(x => x.StatusID).HasColumnName("StatusID").IsRequired();

            // Relationships
            builder.HasOne<MaterialsGroup>(m => m.MaterialsGroup)
                .WithMany()
                .HasForeignKey(m => m.MaterialsGroupID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<MaterialsStatus>(m => m.MaterialsStatus)
                .WithMany()
                .HasForeignKey(m => m.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Unit>(m => m.Unit)
                .WithMany()
                .HasForeignKey(m => m.UnitID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<Unit>(m => m.UnitMax)
                .WithMany()
                .HasForeignKey(m => m.UnitID_Max)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
