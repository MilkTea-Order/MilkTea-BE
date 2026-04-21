using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MilkTea.Domain.Auth.Entities;
using MilkTea.Domain.Auth.ValueObjects;

namespace MilkTea.Infrastructure.Persistence.Configurations.Auth;

public class OtpEntityConfiguration : IEntityTypeConfiguration<OtpEntity>
{
    public void Configure(EntityTypeBuilder<OtpEntity> builder)
    {
        builder.ToTable("OTP");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.SessionId)
            .HasColumnName("SessionId")
            .IsRequired();

        var channelConverter = new ValueConverter<Channel, string>(
            v => v.Value,
            v => Channel.Create(v));

        var statusConverter = new ValueConverter<OtpStatus, string>(
            v => v.Value,
            v => OtpStatus.Create(v));

        builder.Property(x => x.Channel)
            .HasColumnName("Channel")
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion(channelConverter);

        builder.Property(x => x.OtpCode)
            .HasColumnName("OTPCode")
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.CreatedDate)
           .IsRequired();

        builder.Property(x => x.ExpiredDate)
            .HasColumnName("ExpiredDate")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("Status")
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion(statusConverter);

        // Ignore audit fields that are not used
        builder.Ignore(x => x.CreatedBy);
        builder.Ignore(x => x.UpdatedBy);
        builder.Ignore(x => x.UpdatedDate);

        // // Foreign key relationship to Session
        // builder.HasOne<SessionEntity>()
        //     .WithMany()
        //     .HasForeignKey(x => x.SessionId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // Indexes for performance
        builder.HasIndex(x => x.SessionId)
            .HasDatabaseName("IX_Otps_SessionId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_Otps_Status");
    }
}