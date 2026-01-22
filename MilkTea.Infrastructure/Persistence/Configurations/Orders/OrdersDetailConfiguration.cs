using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class OrdersDetailConfiguration : IEntityTypeConfiguration<OrdersDetail>
    {
        public void Configure(EntityTypeBuilder<OrdersDetail> builder)
        {
            builder.ToTable("ordersdetail");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.OrderID).HasColumnName("OrderID").IsRequired();
            builder.Property(x => x.MenuID).HasColumnName("MenuID").IsRequired();
            builder.Property(x => x.Quantity).HasColumnName("Quantity").IsRequired();
            builder.Property(x => x.Price).HasColumnName("Price").IsRequired();
            builder.Property(x => x.PriceListID).HasColumnName("PriceListID");
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(x => x.CancelledBy).HasColumnName("CancelledBy");
            builder.Property(x => x.CancelledDate).HasColumnName("CancelledDate");
            builder.Property(x => x.Note).HasColumnName("Note");
            builder.Property(x => x.KindOfHotpot1ID).HasColumnName("KindOfHotpot1ID");
            builder.Property(x => x.KindOfHotpot2ID).HasColumnName("KindOfHotpot2ID");
            builder.Property(x => x.SizeID).HasColumnName("SizeID").IsRequired();

            // Relationships
            builder.HasOne<Menu>(od => od.Menu)
                .WithMany()
                .HasForeignKey(od => od.MenuID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Size>(od => od.Size)
                .WithMany()
                .HasForeignKey(od => od.SizeID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<PriceList>(od => od.PriceList)
                .WithMany()
                .HasForeignKey(od => od.PriceListID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<KindOfHotpot>(od => od.KindOfHotpot1)
                .WithMany()
                .HasForeignKey(od => od.KindOfHotpot1ID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<KindOfHotpot>(od => od.KindOfHotpot2)
                .WithMany()
                .HasForeignKey(od => od.KindOfHotpot2ID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
