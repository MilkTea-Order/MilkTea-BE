using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class PriceListDetailConfiguration : IEntityTypeConfiguration<PriceListDetail>
    {
        public void Configure(EntityTypeBuilder<PriceListDetail> builder)
        {
            builder.ToTable("pricelistdetail");

            builder.HasKey(x => new { x.PriceListID, x.MenuID, x.SizeID });
            builder.Property(x => x.PriceListID).HasColumnName("PriceListID");
            builder.Property(x => x.MenuID).HasColumnName("MenuID");
            builder.Property(x => x.SizeID).HasColumnName("SizeID");

            builder.Property(x => x.Price).HasColumnName("Price").IsRequired();

            // Relationships
            builder.HasOne<PriceList>(pld => pld.PriceList)
                .WithMany()
                .HasForeignKey(pld => pld.PriceListID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Menu>(pld => pld.Menu)
                .WithMany()
                .HasForeignKey(pld => pld.MenuID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Size>(pld => pld.Size)
                .WithMany()
                .HasForeignKey(pld => pld.SizeID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
