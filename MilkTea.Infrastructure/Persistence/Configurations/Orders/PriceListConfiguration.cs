using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class PriceListConfiguration : IEntityTypeConfiguration<PriceList>
    {
        public void Configure(EntityTypeBuilder<PriceList> builder)
        {
            builder.ToTable("pricelist");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
            builder.Property(x => x.Code).HasColumnName("Code");
            builder.Property(x => x.StartDate).HasColumnName("StartDate").IsRequired();
            builder.Property(x => x.StopDate).HasColumnName("StopDate").IsRequired();
            builder.Property(x => x.CurrencyID).HasColumnName("CurrencyID").IsRequired();
            builder.Property(x => x.StatusOfPriceListID).HasColumnName("StatusOfPriceListID").IsRequired();

            // Relationships
            builder.HasOne<Currency>(pl => pl.Currency)
                .WithMany()
                .HasForeignKey(pl => pl.CurrencyID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<StatusOfPriceList>(pl => pl.StatusOfPriceList)
                .WithMany()
                .HasForeignKey(pl => pl.StatusOfPriceListID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
