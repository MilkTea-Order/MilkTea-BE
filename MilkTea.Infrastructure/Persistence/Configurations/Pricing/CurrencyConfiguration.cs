using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Pricing.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Pricing;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable("currency");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.Symbol).HasColumnName("Symbol");
        builder.Property(x => x.Code).HasColumnName("Code");
    }
}
