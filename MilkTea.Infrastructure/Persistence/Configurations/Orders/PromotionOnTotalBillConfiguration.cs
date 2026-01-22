using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class PromotionOnTotalBillConfiguration : IEntityTypeConfiguration<PromotionOnTotalBill>
    {
        public void Configure(EntityTypeBuilder<PromotionOnTotalBill> builder)
        {
            builder.ToTable("promotionontotalbill");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy");
            builder.Property(x => x.StartDate).HasColumnName("StartDate").IsRequired();
            builder.Property(x => x.StopDate).HasColumnName("StopDate").IsRequired();
            builder.Property(x => x.ProPercent).HasColumnName("ProPercent");
            builder.Property(x => x.ProAmount).HasColumnName("ProAmount");
            builder.Property(x => x.StatusID).HasColumnName("StatusID").IsRequired();
            builder.Property(x => x.Note).HasColumnName("Note");

            // Relationships
            builder.HasOne<StatusOfPromotion>(p => p.StatusOfPromotion)
                .WithMany()
                .HasForeignKey(p => p.StatusID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
