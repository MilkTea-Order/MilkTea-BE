using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Users.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users;

public class PermissionDetailConfiguration : IEntityTypeConfiguration<PermissionDetail>
{
    public void Configure(EntityTypeBuilder<PermissionDetail> builder)
    {
        builder.ToTable("permissiondetail");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID").IsRequired();

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
        builder.Property(x => x.PermissionID).HasColumnName("PermissionID").IsRequired();
        builder.Property(x => x.Note).HasColumnName("Note");
    }
}
