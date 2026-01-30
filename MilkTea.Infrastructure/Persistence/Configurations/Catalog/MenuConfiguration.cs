using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Catalog.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Catalog;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("menu");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.Formula).HasColumnName("Formula");
        builder.Property(x => x.AvatarPicture).HasColumnName("AvatarPicture");
        builder.Property(x => x.Note).HasColumnName("Note");
        builder.Property(x => x.MenuGroupID).HasColumnName("MenuGroupID").IsRequired();

        // Map enum to existing StatusID column
        builder.Property(x => x.Status)
            .HasColumnName("StatusID")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.UnitID).HasColumnName("UnitID").IsRequired();
        builder.Property(x => x.TasteQTy).HasColumnName("TasteQTy");
        builder.Property(x => x.PrintSticker).HasColumnName("PrintSticker");

        builder.HasMany(x => x.MenuSizes)
                        .WithOne()
                        .HasForeignKey("MenuID")
                        .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(x => x.MenuSizes)
            .HasField("_vMenuSizes")
            .UsePropertyAccessMode(PropertyAccessMode.Field);



        builder.Ignore(x => x.CreatedBy);
        builder.Ignore(x => x.CreatedDate);
        builder.Ignore(x => x.UpdatedBy);
        builder.Ignore(x => x.UpdatedDate);
        //builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        //builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        //builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy");
        //builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");

    }
}
