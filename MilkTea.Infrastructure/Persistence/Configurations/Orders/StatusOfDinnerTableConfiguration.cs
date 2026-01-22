using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class StatusOfDinnerTableConfiguration : IEntityTypeConfiguration<StatusOfDinnerTable>
    {
        public void Configure(EntityTypeBuilder<StatusOfDinnerTable> builder)
        {
            builder.ToTable("statusofdinnertable");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        }
    }
}
