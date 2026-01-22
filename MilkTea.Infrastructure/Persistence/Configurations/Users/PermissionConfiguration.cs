using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Users;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("permission");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
            builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
            builder.Property(x => x.PermissionGroupID).HasColumnName("PermissionGroupID").IsRequired();
            builder.Property(x => x.Note).HasColumnName("Note");

            // Relationships
            builder.HasOne<PermissionGroup>(p => p.PermissionGroup)
                .WithMany()
                .HasForeignKey(p => p.PermissionGroupID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
