using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Users.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Identity;

public class UserAndRoleConfiguration : IEntityTypeConfiguration<UserAndRole>
{
    public void Configure(EntityTypeBuilder<UserAndRole> builder)
    {
        builder.ToTable("userandrole");
        builder.HasKey(x => new { x.UserID, x.RoleID });
        builder.Property(x => x.UserID).HasColumnName("UserID").IsRequired();
        builder.Property(x => x.RoleID).HasColumnName("RoleID").IsRequired();
        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
    }
}
