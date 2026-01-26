using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Users.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Identity;

public class RoleDetailConfiguration : IEntityTypeConfiguration<RoleDetail>
{
    public void Configure(EntityTypeBuilder<RoleDetail> builder)
    {
        builder.ToTable("roledetail");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");

        builder.Property(x => x.RoleID).HasColumnName("RoleID").IsRequired();
        builder.Property(x => x.PermissionDetailID).HasColumnName("PermissionDetailID").IsRequired();
    }
}
