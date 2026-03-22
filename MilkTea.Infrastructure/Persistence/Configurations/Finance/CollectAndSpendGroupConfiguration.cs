using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Finance.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Finance
{
    public class CollectAndSpendGroupConfiguration : IEntityTypeConfiguration<CollectAndSpendGroupEntity>
    {
        public void Configure(EntityTypeBuilder<CollectAndSpendGroupEntity> builder)
        {
            builder.ToTable("CollectAndSpendGroup");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID").ValueGeneratedOnAdd();

            builder.Property(x => x.Name).HasColumnName("Name").IsRequired().HasColumnType("text"); ;
        }
    }
}
