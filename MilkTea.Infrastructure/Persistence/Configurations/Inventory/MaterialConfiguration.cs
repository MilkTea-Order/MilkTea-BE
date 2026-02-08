using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Inventory.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Inventory;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("Materials");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.Code).HasColumnName("Code");
        builder.Property(x => x.UnitID).HasColumnName("UnitID");
        builder.Property(x => x.UnitID_Max).HasColumnName("UnitID_Max");
        builder.Property(x => x.StyleQuantity).HasColumnName("StyleQuantity");
        builder.Property(x => x.MaterialsGroupID).HasColumnName("MaterialsGroupID").IsRequired();

        // Map enum to existing StatusID column
        builder.Property(x => x.Status)
            .HasColumnName("StatusID")
            .HasConversion<int>()
            .IsRequired();

        // Relationships
        builder.HasOne(m => m.MaterialsGroup)
            .WithMany(mg => mg.Materials)
            .HasForeignKey(m => m.MaterialsGroupID)
            .OnDelete(DeleteBehavior.Restrict);

        //builder.HasOne(m => m.Unit)
        //    .WithMany()
        //    .HasForeignKey(m => m.UnitID)
        //    .OnDelete(DeleteBehavior.Restrict);

        //builder.HasOne(m => m.UnitMax)
        //    .WithMany()
        //    .HasForeignKey(m => m.UnitID_Max)
        //    .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy");
        builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
    }
}
