using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Catalog.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Catalog;

public class PromotionOnTotalBillConfiguration : IEntityTypeConfiguration<PromotionOnTotalBill>
{
    public void Configure(EntityTypeBuilder<PromotionOnTotalBill> builder)
    {
        builder.ToTable("PromotionOnTotalBill");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy");
        builder.Property(x => x.StartDate).HasColumnName("StartDate").IsRequired();
        builder.Property(x => x.StopDate).HasColumnName("StopDate").IsRequired();
        builder.Property(x => x.ProPercent).HasColumnName("ProPercent");
        builder.Property(x => x.ProAmount).HasColumnName("ProAmount");

        // Map enum to existing StatusID column
        builder.Property(x => x.Status)
            .HasColumnName("StatusID")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Note).HasColumnName("Note");

        builder.Ignore(x => x.DomainEvents);
    }
}
