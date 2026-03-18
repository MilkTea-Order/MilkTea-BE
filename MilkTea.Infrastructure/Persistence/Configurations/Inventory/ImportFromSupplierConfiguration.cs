using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Inventory.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Inventory
{
    public class ImportFromSupplierConfiguration : IEntityTypeConfiguration<ImportFromSupplierEntity>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ImportFromSupplierEntity> builder)
        {
            builder.ToTable("ImportFromSuppliers");

            builder.HasKey(e => e.Id);
            builder.Property(x => x.Id).HasColumnName("ID");

            builder.Property(x => x.SupplierId).HasColumnName("SuppliersID").IsRequired();

            builder.Property(e => e.BillNo).HasMaxLength(50);
            builder.Property(e => e.BillDate);

            builder.Property(e => e.ImportedDate).IsRequired();
            builder.Property(e => e.ImportedBy).IsRequired();

            builder.Property(e => e.Note).HasMaxLength(200);


            builder.Property(x => x.Status).HasColumnName("StatusID")
                                            .HasConversion<int>()
                                            .IsRequired();

            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.CreatedDate).IsRequired();

            builder.Property(e => e.UpdatedBy).HasColumnName("LastUpdatedBy");
            builder.Property(e => e.UpdatedDate).HasColumnName("LastUpdatedDate");


            builder.Property(e => e.ApprovedBy);
            builder.Property(e => e.ApprovedDate);
        }
    }
}