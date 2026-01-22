using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Users;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.EmployeesID).HasColumnName("EmployeesID").IsRequired();
            builder.Property(x => x.UserName).HasColumnName("UserName").IsRequired();
            builder.Property(x => x.Password).HasColumnName("Password").IsRequired();
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate");
            builder.Property(x => x.StoppedBy).HasColumnName("StoppedBy");
            builder.Property(x => x.StoppedDate).HasColumnName("StoppedDate");
            builder.Property(x => x.LastUpdatedBy).HasColumnName("LastUpdatedBy");
            builder.Property(x => x.LastUpdatedDate).HasColumnName("LastUpdatedDate");
            builder.Property(x => x.PasswordResetBy).HasColumnName("Password_ResetBy");
            builder.Property(x => x.PasswordResetDate).HasColumnName("Password_ResetDate");
            builder.Property(x => x.StatusID).HasColumnName("StatusID").IsRequired();

            // Relationships
            builder.HasOne<Employee>(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<User>(u => u.EmployeesID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(u => u.EmployeesID)
                .IsUnique();

            builder.HasOne<Status>(u => u.Status)
                .WithMany()
                .HasForeignKey(u => u.StatusID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
