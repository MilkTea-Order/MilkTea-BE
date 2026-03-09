using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Catalog.Material.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Catalog.Material;

public class MaterialGroupConfiguration : IEntityTypeConfiguration<MaterialsGroupEntity>
{
    public void Configure(EntityTypeBuilder<MaterialsGroupEntity> builder)
    {
        builder.ToTable("MaterialsGroup");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
    }
}
