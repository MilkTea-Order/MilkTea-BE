using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class KindOfHotpotConfiguration : IEntityTypeConfiguration<KindOfHotpot>
    {
        public void Configure(EntityTypeBuilder<KindOfHotpot> builder)
        {
            builder.ToTable("kindofhotpot");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        }
    }
}
