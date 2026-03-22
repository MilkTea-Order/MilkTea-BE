using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Finance.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Finance
{
    public class CollectAndSpendConfiguration : IEntityTypeConfiguration<CollectAndSpendEntity>
    {
        public void Configure(EntityTypeBuilder<CollectAndSpendEntity> builder)
        {
            builder.ToTable("CollectAndSpend");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID").ValueGeneratedOnAdd();

            builder.Property(x => x.CollectAndSpendGroupID).HasColumnName("CollectAndSpendGroupID").IsRequired();
            builder.Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(255);

            builder.Property(x => x.ActionDate).HasColumnName("ActionDate").IsRequired();
            builder.Property(x => x.ActionBy).HasColumnName("ActionBy").IsRequired();

            builder.Property(x => x.Amount).HasColumnName("Amount").IsRequired().HasColumnType("decimal(10,0)");
            builder.Property(x => x.Note).HasColumnName("Note").HasColumnType("longtext");

            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy");
            builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
        }
    }
}
