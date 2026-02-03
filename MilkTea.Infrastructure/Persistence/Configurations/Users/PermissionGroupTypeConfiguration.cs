using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Users.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users;

public class PermissionGroupTypeConfiguration : IEntityTypeConfiguration<PermissionGroupType>
{
    public void Configure(EntityTypeBuilder<PermissionGroupType> builder)
    {
        builder.ToTable("PermissionGroupType");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.Note).HasColumnName("Note");

        builder.HasMany(x => x.PermissionGroups)
            .WithOne()
            .HasForeignKey(pg => pg.PermissionGroupTypeID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(x => x.PermissionGroups)
            .HasField("_vPermissionGroups")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
