using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Pricing.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Pricing;

public class PriceListConfiguration : IEntityTypeConfiguration<PriceList>
{
    public void Configure(EntityTypeBuilder<PriceList> builder)
    {
        builder.ToTable("pricelist");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.Code).HasColumnName("Code");
        builder.Property(x => x.StartDate).HasColumnName("StartDate").IsRequired();
        builder.Property(x => x.StopDate).HasColumnName("StopDate").IsRequired();
        builder.Property(x => x.CurrencyID).HasColumnName("CurrencyID").IsRequired();

        // Map enum to existing StatusOfPriceListID column
        builder.Property(x => x.Status)
            .HasColumnName("StatusOfPriceListID")
            .HasConversion<int>()
            .IsRequired();

        // Relationships
        builder.HasOne(pl => pl.Currency)
            .WithMany()
            .HasForeignKey(pl => pl.CurrencyID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(pl => pl.Details)
                .WithOne(d => d.PriceList)
                .HasForeignKey(d => d.PriceListID)
                .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Details)
            .HasField("_vDetails")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(x => x.CreatedBy);
        builder.Ignore(x => x.CreatedDate);
        builder.Ignore(x => x.UpdatedBy);
        builder.Ignore(x => x.UpdatedDate);
    }
}
