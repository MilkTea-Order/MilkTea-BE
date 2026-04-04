using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Auth.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Auth;

public class RoleDetailConfiguration : IEntityTypeConfiguration<RoleDetailEntity>
{
    public void Configure(EntityTypeBuilder<RoleDetailEntity> builder)
    {
        builder.ToTable("RoleDetail");
        builder.HasKey(x => new { x.RoleID, x.PermissionDetailID });

        builder.Property(x => x.RoleID).HasColumnName("RoleID").IsRequired();
        builder.Property(x => x.PermissionDetailID).HasColumnName("PermissionDetailID").IsRequired();
        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
    }
}
