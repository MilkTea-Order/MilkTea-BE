using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Auth.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Auth;

public class UserAndPermissionDetailConfiguration : IEntityTypeConfiguration<UserAndPermissionDetailEntity>
{
    public void Configure(EntityTypeBuilder<UserAndPermissionDetailEntity> builder)
    {
        builder.ToTable("UserAndPermissionDetail");

        builder.HasKey(x => new { x.UserID, x.PermissionDetailID });

        builder.Property(x => x.UserID)
            .HasColumnName("UserID")
            .IsRequired();

        builder.Property(x => x.PermissionDetailID)
            .HasColumnName("PermissionDetailID")
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasColumnName("CreatedBy")
            .IsRequired();

        builder.Property(x => x.CreatedDate)
            .HasColumnName("CreatedDate")
            .IsRequired();
    }
}
