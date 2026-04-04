using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Auth.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Auth;

public class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
{
    public void Configure(EntityTypeBuilder<PermissionEntity> builder)
    {
        builder.ToTable("Permission");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
        builder.Property(x => x.PermissionGroupID).HasColumnName("PermissionGroupID").IsRequired();
        builder.Property(x => x.Note).HasColumnName("Note");

        builder.HasMany(p => p.PermissionDetails)
            .WithOne()
            .HasForeignKey(pd => pd.PermissionID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.PermissionDetails)
            .HasField("_vPermissionDetails")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
