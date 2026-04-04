using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Auth.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Auth;

public class UserAndRoleConfiguration : IEntityTypeConfiguration<UserAndRoleEntity>
{
    public void Configure(EntityTypeBuilder<UserAndRoleEntity> builder)
    {
        builder.ToTable("UserAndRole");

        builder.HasKey(x => new { x.UserID, x.RoleID });

        builder.Property(x => x.UserID)
            .HasColumnName("UserID")
            .IsRequired();

        builder.Property(x => x.RoleID)
            .HasColumnName("RoleID")
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasColumnName("CreatedBy")
            .IsRequired();

        builder.Property(x => x.CreatedDate)
            .HasColumnName("CreatedDate")
            .IsRequired();

        builder.HasOne<RoleEntity>()
            .WithMany()
            .HasForeignKey(x => x.RoleID)
            .HasPrincipalKey(r => r.Id);
    }
}
