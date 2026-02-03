using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Users.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users;

public class UserAndPermissionDetailConfiguration : IEntityTypeConfiguration<UserAndPermissionDetail>
{
    public void Configure(EntityTypeBuilder<UserAndPermissionDetail> builder)
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
