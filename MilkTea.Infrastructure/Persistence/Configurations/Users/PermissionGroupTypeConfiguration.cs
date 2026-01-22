using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Users;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users
{
    public class PermissionGroupTypeConfiguration : IEntityTypeConfiguration<PermissionGroupType>
    {
        public void Configure(EntityTypeBuilder<PermissionGroupType> builder)
        {
            builder.ToTable("permissiongrouptype");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
            builder.Property(x => x.Note).HasColumnName("Note");
        }
    }
}
