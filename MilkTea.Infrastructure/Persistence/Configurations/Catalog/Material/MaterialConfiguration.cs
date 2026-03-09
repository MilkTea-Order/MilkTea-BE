using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Catalog.Material.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Catalog.Material;

public class MaterialConfiguration : IEntityTypeConfiguration<MaterialEntity>
{
    public void Configure(EntityTypeBuilder<MaterialEntity> builder)
    {
        builder.ToTable("Materials");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID").ValueGeneratedOnAdd();

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.Code).HasColumnName("Code");
        builder.Property(x => x.UnitID).HasColumnName("UnitID");
        builder.Property(x => x.UnitID_Max).HasColumnName("UnitID_Max");
        builder.Property(x => x.StyleQuantity).HasColumnName("StyleQuantity");
        builder.Property(x => x.MaterialsGroupID).HasColumnName("MaterialsGroupID").IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("StatusID")
            .HasConversion<int>()
            .IsRequired();

        builder.Ignore(x => x.CreatedBy);
        builder.Ignore(x => x.CreatedDate);
        builder.Ignore(x => x.UpdatedBy);
        builder.Ignore(x => x.UpdatedDate);
    }
}
