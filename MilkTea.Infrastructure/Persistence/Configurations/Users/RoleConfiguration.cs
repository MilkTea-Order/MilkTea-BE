using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Users.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("role");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.Note).HasColumnName("Note");


        builder.Property(x => x.Status)
            .HasColumnName("StatusID")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("LastUpdatedBy");
        builder.Property(x => x.UpdatedDate).HasColumnName("LastUpdatedDate");

        builder.HasMany(r => r.RoleDetails)
            .WithOne()
            .HasForeignKey(rd => rd.RoleID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.RoleDetails)
            .HasField("_vRoleDetails")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
