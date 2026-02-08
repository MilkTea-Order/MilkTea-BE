using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Catalog.Entities.Price;

namespace MilkTea.Infrastructure.Persistence.Configurations.Catalog;

public class PriceListDetailConfiguration : IEntityTypeConfiguration<PriceListDetailEntity>
{
    public void Configure(EntityTypeBuilder<PriceListDetailEntity> builder)
    {
        builder.ToTable("PriceListDetail");

        builder.HasKey(x => new { x.PriceListID, x.MenuID, x.SizeID });

        builder.Property(x => x.PriceListID).HasColumnName("PriceListID").IsRequired();
        builder.Property(x => x.MenuID).HasColumnName("MenuID").IsRequired();
        builder.Property(x => x.SizeID).HasColumnName("SizeID").IsRequired();
        builder.Property(x => x.Price).HasColumnName("Price").IsRequired();

        //builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        //builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        //builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy");
        //builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Ignore(x => x.Id);
        builder.Ignore(x => x.CreatedBy);
        builder.Ignore(x => x.CreatedDate);
        builder.Ignore(x => x.UpdatedBy);
        builder.Ignore(x => x.UpdatedDate);
    }
}
