using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class StatusOfPriceListConfiguration : IEntityTypeConfiguration<StatusOfPriceList>
    {
        public void Configure(EntityTypeBuilder<StatusOfPriceList> builder)
        {
            builder.ToTable("statusofpricelist");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        }
    }
}
