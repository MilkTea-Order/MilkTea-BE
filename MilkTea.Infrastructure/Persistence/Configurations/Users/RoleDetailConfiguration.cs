using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Users;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users
{
    public class RoleDetailConfiguration : IEntityTypeConfiguration<RoleDetail>
    {
        public void Configure(EntityTypeBuilder<RoleDetail> builder)
        {
            builder.ToTable("roledetail");

            builder.HasKey(x => new { x.PermissionDetailID, x.RoleID });
            builder.Property(x => x.PermissionDetailID).HasColumnName("PermissionDetailID");
            builder.Property(x => x.RoleID).HasColumnName("RoleID");

            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();

            // Relationships
            builder.HasOne<Role>(rd => rd.Role)
                .WithMany()
                .HasForeignKey(rd => rd.RoleID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<PermissionDetail>(rd => rd.PermissionDetail)
                .WithMany()
                .HasForeignKey(rd => rd.PermissionDetailID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
