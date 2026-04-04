using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Auth.Entities;

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

        builder.Property(x => x.Email)
            .HasColumnName("Email")
            .HasMaxLength(100);


        builder.Property(x => x.OtpCode)
            .HasColumnName("OTPCode")
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.OTPDate)
            .HasColumnName("OTPDate")
            .IsRequired();

        builder.Property(x => x.OTPType)
            .HasColumnName("OTPType")
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.NumOfTimes)
            .HasColumnName("NumOfTimes")
            .IsRequired()
            .HasDefaultValue(0);


        // Ignore audit fields that are not used
        builder.Ignore(x => x.CreatedBy);
        builder.Ignore(x => x.CreatedDate);
        builder.Ignore(x => x.UpdatedBy);
        builder.Ignore(x => x.UpdatedDate);


        // Indexes for performance
        builder.HasIndex(x => x.Email)
            .HasDatabaseName("IX_Otps_Email");


        builder.HasIndex(x => new { x.Email, x.OTPType })
            .HasDatabaseName("IX_Otps_Email_OTPType");

    }
}
