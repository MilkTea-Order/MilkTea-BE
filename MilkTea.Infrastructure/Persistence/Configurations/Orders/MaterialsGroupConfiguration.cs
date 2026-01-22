using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class MaterialsGroupConfiguration : IEntityTypeConfiguration<MaterialsGroup>
    {
        public void Configure(EntityTypeBuilder<MaterialsGroup> builder)
        {
            builder.ToTable("materialsgroup");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        }
    }
}
