using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Auth.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Auth;

public class ResetPasswordTokenConfiguration : IEntityTypeConfiguration<ResetPasswordTokenEntity>
{
    public void Configure(EntityTypeBuilder<ResetPasswordTokenEntity> builder)
    {
        builder.ToTable("ResetTokens");

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

        builder.Property(x => x.IsUsed)
            .HasColumnName("IsUsed")
            .IsRequired();

        builder.Property(x => x.IsRevoked)
            .HasColumnName("IsRevoked")
            .IsRequired();

        builder.Property(x => x.UsedAt)
            .HasColumnName("UsedAt");

        builder.Property(x => x.CreatedDate)
            .HasColumnName("CreatedDate")
            .IsRequired();
        builder.Property(x => x.UpdatedDate)
            .HasColumnName("LastUpdatedDate");

        builder.Ignore(x => x.CreatedBy);
        builder.Ignore(x => x.UpdatedBy);

        // Ignore computed properties - these are calculated in memory
        builder.Ignore(x => x.IsExpired);
        builder.Ignore(x => x.IsValid);
        builder.Ignore(x => x.DomainEvents);

        // Indexes
        builder.HasIndex(x => x.Token)
            .HasDatabaseName("IX_ResetPasswordTokens_Token")
            .IsUnique();

        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("IX_ResetPasswordTokens_UserId");
    }
}