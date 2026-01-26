using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Pricing.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Pricing;

public class PriceListDetailConfiguration : IEntityTypeConfiguration<PriceListDetail>
{
    public void Configure(EntityTypeBuilder<PriceListDetail> builder)
    {
        builder.ToTable("pricelistdetail");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.PriceListID).HasColumnName("PriceListID").IsRequired();
        builder.Property(x => x.MenuID).HasColumnName("MenuID").IsRequired();
        builder.Property(x => x.SizeID).HasColumnName("SizeID").IsRequired();
        builder.Property(x => x.Price).HasColumnName("Price").IsRequired();

        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy");
        builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
    }
}
