using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Users.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users;

public class RoleDetailConfiguration : IEntityTypeConfiguration<RoleDetail>
{
    public void Configure(EntityTypeBuilder<RoleDetail> builder)
    {
        builder.ToTable("RoleDetail");
        builder.HasKey(x => new { x.RoleID, x.PermissionDetailID });

        builder.Property(x => x.RoleID).HasColumnName("RoleID").IsRequired();
        builder.Property(x => x.PermissionDetailID).HasColumnName("PermissionDetailID").IsRequired();
        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
    }
}
