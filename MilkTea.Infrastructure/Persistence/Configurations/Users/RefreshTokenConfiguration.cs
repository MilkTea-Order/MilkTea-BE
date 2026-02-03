using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Users.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .IsRequired();

        builder.Property(x => x.Token)
            .HasColumnName("Token")
            .IsRequired();

        builder.Property(x => x.ExpiryDate)
            .HasColumnName("ExpiryDate")
            .IsRequired();

        builder.Property(x => x.IsRevoked)
            .HasColumnName("IsRevoked")
            .IsRequired();

        // Audit fields
        builder.Property(x => x.CreatedDate)
            .HasColumnName("CreatedDate")
            .IsRequired();

        builder.Property(x => x.UpdatedDate)
            .HasColumnName("LastUpdatedDate");

        // Ignore some fields 
        builder.Ignore(x => x.CreatedBy);
        builder.Ignore(x => x.UpdatedBy);

        // Ignore computed properties - these are calculated and not stored in the database
        builder.Ignore(x => x.IsExpired);
        builder.Ignore(x => x.IsValid);
    }
}
