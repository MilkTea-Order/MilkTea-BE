using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class DinnerTableConfiguration : IEntityTypeConfiguration<DinnerTable>
    {
        public void Configure(EntityTypeBuilder<DinnerTable> builder)
        {
            builder.ToTable("dinnertable");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Code).HasColumnName("Code");
            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
            builder.Property(x => x.Position).HasColumnName("Position");
            builder.Property(x => x.NumberOfSeats).HasColumnName("NumberOfSeats").IsRequired();
            builder.Property(x => x.Longs).HasColumnName("Longs");
            builder.Property(x => x.Width).HasColumnName("Width");
            builder.Property(x => x.Height).HasColumnName("Height");
            builder.Property(x => x.EmptyPicture).HasColumnName("EmptyPicture").IsRequired();
            builder.Property(x => x.UsingPicture).HasColumnName("UsingPicture").IsRequired();
            builder.Property(x => x.StatusOfDinnerTableID).HasColumnName("StatusOfDinnerTableID").IsRequired();
            builder.Property(x => x.Note).HasColumnName("Note");

            // Relationships
            builder.HasOne<StatusOfDinnerTable>(dt => dt.StatusOfDinnerTable)
                .WithMany()
                .HasForeignKey(dt => dt.StatusOfDinnerTableID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
