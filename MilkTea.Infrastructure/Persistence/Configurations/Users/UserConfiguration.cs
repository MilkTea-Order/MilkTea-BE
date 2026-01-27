using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Users.Entities;
using MilkTea.Domain.Users.ValueObject;

namespace MilkTea.Infrastructure.Persistence.Configurations.Identity;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeID)
            .HasColumnName("EmployeesID")
            .IsRequired();

        // Map UserName Value Object as Complex Property
        builder.ComplexProperty(x => x.UserName, un =>
        {
            un.Property(u => u.value)
                .HasColumnName("UserName")
                .IsRequired();
        });

        // Map Password Value Object as Complex Property
        builder.ComplexProperty(x => x.Password, p =>
        {
            p.Property(pw => pw.value)
                .HasColumnName("Password")
                .IsRequired();
        });

        // Audit fields
        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(x => x.StoppedBy).HasColumnName("StoppedBy");
        builder.Property(x => x.StoppedDate).HasColumnName("StoppedDate");
        builder.Property(x => x.UpdatedBy).HasColumnName("LastUpdatedBy");
        builder.Property(x => x.UpdatedDate).HasColumnName("LastUpdatedDate");
        builder.Property(x => x.PasswordResetBy).HasColumnName("Password_ResetBy");
        builder.Property(x => x.PasswordResetDate).HasColumnName("Password_ResetDate");

        // Map enum to existing StatusID column
        builder.Property(x => x.Status)
            .HasColumnName("StatusID")
            .HasConversion<int>()
            .IsRequired();

        // Cross-aggregate reference to Employee (no navigation in User aggregate)
        builder.HasIndex(u => u.EmployeeID).IsUnique();

        // Child entity: RefreshToken
        builder.HasMany(u => u.RefreshTokens)
            .WithOne()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(u => u.RefreshTokens)
            .HasField("_vRefreshTokens")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(x => x.DomainEvents);
    }
}
