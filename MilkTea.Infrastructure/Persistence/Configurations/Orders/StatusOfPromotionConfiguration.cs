using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class StatusOfPromotionConfiguration : IEntityTypeConfiguration<StatusOfPromotion>
    {
        public void Configure(EntityTypeBuilder<StatusOfPromotion> builder)
        {
            builder.ToTable("statusofpromotion");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        }
    }
}
