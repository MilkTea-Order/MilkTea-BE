using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Catalog.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Catalog;

public class SizeConfiguration : IEntityTypeConfiguration<Size>
{
    public void Configure(EntityTypeBuilder<Size> builder)
    {
        builder.ToTable("size");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.RankIndex).HasColumnName("RankIndex").IsRequired();

        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy");
        builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
    }
}
