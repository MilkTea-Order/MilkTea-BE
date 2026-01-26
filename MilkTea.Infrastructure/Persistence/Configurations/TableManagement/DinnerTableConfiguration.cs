using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Catalog.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.TableManagement;

public class DinnerTableConfiguration : IEntityTypeConfiguration<DinnerTable>
{
    public void Configure(EntityTypeBuilder<DinnerTable> builder)
    {
        builder.ToTable("dinnertable");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Code).HasColumnName("Code");
        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.Position).HasColumnName("Position");
        builder.Property(x => x.NumberOfSeats).HasColumnName("NumberOfSeats").IsRequired();
        builder.Property(x => x.Longs).HasColumnName("Longs");
        builder.Property(x => x.Width).HasColumnName("Width");
        builder.Property(x => x.Height).HasColumnName("Height");
        builder.Property(x => x.EmptyPicture).HasColumnName("EmptyPicture");
        builder.Property(x => x.UsingPicture).HasColumnName("UsingPicture");

        // Map enum to existing StatusOfDinnerTableID column
        builder.Property(x => x.Status)
            .HasColumnName("StatusOfDinnerTableID")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Note).HasColumnName("Note");


        builder.Ignore(x => x.CreatedBy);
        builder.Ignore(x => x.CreatedDate);
        builder.Ignore(x => x.UpdatedBy);
        builder.Ignore(x => x.UpdatedDate);
    }
}
