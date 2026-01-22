using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Entities.Users;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class MenuGroupConfiguration : IEntityTypeConfiguration<MenuGroup>
    {
        public void Configure(EntityTypeBuilder<MenuGroup> builder)
        {
            builder.ToTable("menugroup");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
            builder.Property(x => x.StatusID).HasColumnName("StatusID").IsRequired();

            // Relationships
            builder.HasOne<Status>(mg => mg.Status)
                .WithMany()
                .HasForeignKey(mg => mg.StatusID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
