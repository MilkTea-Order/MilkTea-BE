using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Inventory.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Inventory;

public class MaterialsGroupConfiguration : IEntityTypeConfiguration<MaterialsGroup>
{
    public void Configure(EntityTypeBuilder<MaterialsGroup> builder)
    {
        builder.ToTable("materialsgroup");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.Note).HasColumnName("Note");
    }
}
