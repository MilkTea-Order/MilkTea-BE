using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Users;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users
{
    public class UserAndPermissionDetailConfiguration : IEntityTypeConfiguration<UserAndPermissionDetail>
    {
        public void Configure(EntityTypeBuilder<UserAndPermissionDetail> builder)
        {
            builder.ToTable("userandpermissiondetail");

            builder.HasKey(x => new { x.UserID, x.PermissionDetailID });
            builder.Property(x => x.UserID).HasColumnName("UserID");
            builder.Property(x => x.PermissionDetailID).HasColumnName("PermissionDetailID");

            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();

            // Relationships
            builder.HasOne<User>(up => up.User)
                .WithMany()
                .HasForeignKey(up => up.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<PermissionDetail>(up => up.PermissionDetail)
                .WithMany()
                .HasForeignKey(up => up.PermissionDetailID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
