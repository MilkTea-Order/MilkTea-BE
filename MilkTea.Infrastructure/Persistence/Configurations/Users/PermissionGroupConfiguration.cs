using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Users.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users;

public class PermissionGroupConfiguration : IEntityTypeConfiguration<PermissionGroup>
{
    public void Configure(EntityTypeBuilder<PermissionGroup> builder)
    {
        builder.ToTable("permissiongroup");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.PermissionGroupTypeID).HasColumnName("PermissionGroupTypeID").IsRequired();
        builder.Property(x => x.Note).HasColumnName("Note");

        builder.HasMany(x => x.Permissions)
            .WithOne()
            .HasForeignKey(p => p.PermissionGroupID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(x => x.Permissions)
            .HasField("_vPermissions")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
