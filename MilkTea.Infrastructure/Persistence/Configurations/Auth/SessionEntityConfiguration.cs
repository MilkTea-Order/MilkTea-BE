using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MilkTea.Domain.Auth.Entities;
using MilkTea.Domain.Auth.ValueObjects;

namespace MilkTea.Infrastructure.Persistence.Configurations.Auth;

public class SessionEntityConfiguration : IEntityTypeConfiguration<SessionEntity>
{
    public void Configure(EntityTypeBuilder<SessionEntity> builder)
    {
        builder.ToTable("Session");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Email)
            .HasColumnName("DestinationEmail")
            .HasMaxLength(100);

        builder.Property(x => x.Phone)
            .HasColumnName("DestinationPhone")
            .HasMaxLength(100);

        // Value Converters for Value Objects → string in DB
        var channelConverter = new ValueConverter<Channel, string>(
            v => v.Value,
            v => Channel.Create(v));

        var functionConverter = new ValueConverter<SessionFunction, string>(
            v => v.Value,
            v => SessionFunction.Create(v));

        var statusConverter = new ValueConverter<SessionStatus, string>(
            v => v.Value,
            v => SessionStatus.Create(v));

        builder.Property(x => x.Channel)
            .HasColumnName("PreferredChannel")
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion(channelConverter);

        builder.Property(x => x.Function)
            .HasColumnName("Function")
            .IsRequired()
            .HasMaxLength(30)
            .HasConversion(functionConverter);

        builder.Property(x => x.Status)
            .HasColumnName("Status")
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion(statusConverter);

        builder.Property(x => x.CreatedDate).IsRequired();

        builder.Property(x => x.ExpiresDate)
            .HasColumnName("ExpiresDate")
            .IsRequired();

        builder.Property(x => x.VerifiedDate)
            .HasColumnName("VerifiedDate");

        // Ignore audit fields that are not used
        builder.Ignore(x => x.CreatedBy);
        builder.Ignore(x => x.UpdatedBy);
        builder.Ignore(x => x.UpdatedDate);

        // Indexes
        builder.HasIndex(x => x.Email)
            .HasDatabaseName("IX_Sessions_Email");

        builder.HasIndex(x => x.Function)
            .HasDatabaseName("IX_Sessions_Function");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_Sessions_Status");
    }
}
