using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Users;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users
{
    public class PermissionGroupConfiguration : IEntityTypeConfiguration<PermissionGroup>
    {
        public void Configure(EntityTypeBuilder<PermissionGroup> builder)
        {
            builder.ToTable("permissiongroup");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
            builder.Property(x => x.PermissionGroupTypeID).HasColumnName("PermissionGroupTypeID").IsRequired();
            builder.Property(x => x.Note).HasColumnName("Note");

            // Relationships
            builder.HasOne<PermissionGroupType>(pg => pg.PermissionGroupType)
                .WithMany()
                .HasForeignKey(pg => pg.PermissionGroupTypeID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
