using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.DinnerTableID).HasColumnName("DinnerTableID").IsRequired();
            builder.Property(x => x.OrderBy).HasColumnName("OrderBy").IsRequired();
            builder.Property(x => x.OrderDate).HasColumnName("OrderDate").IsRequired();
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy");
            builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(x => x.CancelledBy).HasColumnName("CancelledBy");
            builder.Property(x => x.CancelledDate).HasColumnName("CancelledDate");
            builder.Property(x => x.StatusOfOrderID).HasColumnName("StatusOfOrderID").IsRequired();
            builder.Property(x => x.Note).HasColumnName("Note");
            builder.Property(x => x.PaymentedBy).HasColumnName("PaymentedBy");
            builder.Property(x => x.PaymentedDate).HasColumnName("PaymentedDate");
            builder.Property(x => x.PaymentedTotal).HasColumnName("PaymentedTotal");
            builder.Property(x => x.PaymentedType).HasColumnName("PaymentedType");
            builder.Property(x => x.AddNoteBy).HasColumnName("AddNoteBy");
            builder.Property(x => x.AddNoteDate).HasColumnName("AddNoteDate");
            builder.Property(x => x.ChangeBy).HasColumnName("ChangeBy");
            builder.Property(x => x.ChangeDate).HasColumnName("ChangeDate");
            builder.Property(x => x.MergedBy).HasColumnName("MergedBy");
            builder.Property(x => x.MergedDate).HasColumnName("MergedDate");
            builder.Property(x => x.BillNo).HasColumnName("BillNo");
            builder.Property(x => x.PromotionID).HasColumnName("PromotionID");
            builder.Property(x => x.PromotionPercent).HasColumnName("PromotionPercent");
            builder.Property(x => x.PromotionAmount).HasColumnName("PromotionAmount");
            builder.Property(x => x.TotalAmount).HasColumnName("TotalAmount");
            builder.Property(x => x.ActionBy).HasColumnName("ActionBy");
            builder.Property(x => x.ActionDate).HasColumnName("ActionDate");

            // Relationships
            builder.HasOne<DinnerTable>(o => o.DinnerTable)
                .WithMany()
                .HasForeignKey(o => o.DinnerTableID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<StatusOfOrder>(o => o.StatusOfOrder)
                .WithMany()
                .HasForeignKey(o => o.StatusOfOrderID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<PromotionOnTotalBill>(o => o.PromotionOnTotalBill)
                .WithMany()
                .HasForeignKey(o => o.PromotionID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany<OrdersDetail>(o => o.OrdersDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
