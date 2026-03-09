using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Catalog.Menu.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Catalog.Menu;

public class KindOfHotpotConfiguration : IEntityTypeConfiguration<KindOfHotpot>
{
    public void Configure(EntityTypeBuilder<KindOfHotpot> builder)
    {
        builder.ToTable("KindOfHotpot");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
    }
}
