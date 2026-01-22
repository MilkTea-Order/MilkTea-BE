using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Users;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("role");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
            builder.Property(x => x.Note).HasColumnName("Note");
            builder.Property(x => x.StatusID).HasColumnName("StatusID").IsRequired();
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(x => x.LastUpdatedBy).HasColumnName("LastUpdatedBy");
            builder.Property(x => x.LastUpdatedDate).HasColumnName("LastUpdatedDate");

            // Relationships
            builder.HasOne<Status>(r => r.Status)
                .WithMany()
                .HasForeignKey(r => r.StatusID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
