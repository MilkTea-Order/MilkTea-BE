using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Catalog.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Catalog;

public class MenuGroupConfiguration : IEntityTypeConfiguration<MenuGroup>
{
    public void Configure(EntityTypeBuilder<MenuGroup> builder)
    {
        builder.ToTable("menugroup");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();

        // Map enum to existing StatusID column
        builder.Property(x => x.Status)
            .HasColumnName("StatusID")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy");
        builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");

        builder.HasMany(x => x.Menus)
            .WithOne("MenuGroup")
            .HasForeignKey("MenuGroupID")
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(x => x.Menus)
            .HasField("_vMenus")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
