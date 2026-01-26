using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Users.Entities;

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

        builder.Property(x => x.EmployeesID).HasColumnName("EmployeesID").IsRequired();
        builder.Property(x => x.UserName).HasColumnName("UserName").IsRequired();
        builder.Property(x => x.Password).HasColumnName("Password").IsRequired();
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

        // Relationships
        builder.HasOne(u => u.Employee)
            .WithOne(e => e.User)
            .HasForeignKey<User>(u => u.EmployeesID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(u => u.EmployeesID).IsUnique();

        //builder.HasMany(typeof(RefreshToken), "_refreshTokens")
        //    .WithOne("User")
        //    .HasForeignKey("UserId")
        //    .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(u => u.RefreshTokens)
            .HasField("_vRefreshTokens")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(x => x.DomainEvents);

        builder.HasMany(u => u.UserRoles)
            .WithOne()
            .HasForeignKey(ur => ur.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(u => u.UserRoles)
            .HasField("_vUserRoles")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(u => u.UserPermissions)
            .WithOne()
            .HasForeignKey(up => up.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(u => u.UserPermissions)
            .HasField("_vUserPermissions")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
