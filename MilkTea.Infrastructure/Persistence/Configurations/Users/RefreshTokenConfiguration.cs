using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Users;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refreshtokens");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID");

            builder.Property(x => x.UserId).HasColumnName("UserID").IsRequired();
            builder.Property(x => x.Token).HasColumnName("Token").IsRequired().HasMaxLength(500);
            builder.Property(x => x.ExpiryDate).HasColumnName("ExpiryDate").IsRequired();
            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate");
            builder.Property(x => x.LastUpdatedDate).HasColumnName("LastUpdatedDate");
            builder.Property(x => x.IsRevoked).HasColumnName("IsRevoked").IsRequired();

            // Relationships
            builder.HasOne<User>(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
