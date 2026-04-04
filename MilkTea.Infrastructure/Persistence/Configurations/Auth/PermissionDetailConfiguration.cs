using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Auth.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Auth;

public class PermissionDetailConfiguration : IEntityTypeConfiguration<PermissionDetailEntity>
{
    public void Configure(EntityTypeBuilder<PermissionDetailEntity> builder)
    {
        builder.ToTable("PermissionDetail");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID").IsRequired();

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
        builder.Property(x => x.PermissionID).HasColumnName("PermissionID").IsRequired();
        builder.Property(x => x.Note).HasColumnName("Note");
    }
}
