using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Users;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users
{
    public class UserAndRoleConfiguration : IEntityTypeConfiguration<UserAndRole>
    {
        public void Configure(EntityTypeBuilder<UserAndRole> builder)
        {
            builder.ToTable("userandrole");

            builder.HasKey(x => new { x.UserID, x.RoleID });
            builder.Property(x => x.UserID).HasColumnName("UserID");
            builder.Property(x => x.RoleID).HasColumnName("RoleID");

            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();

            // Relationships
            builder.HasOne<User>(ur => ur.User)
                .WithMany()
                .HasForeignKey(ur => ur.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Role>(ur => ur.Role)
                .WithMany()
                .HasForeignKey(ur => ur.RoleID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
