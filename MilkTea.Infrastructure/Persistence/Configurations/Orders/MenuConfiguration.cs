using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Entities.Users;

namespace MilkTea.Infrastructure.Persistence.Configurations.Orders
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("menu");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
            builder.Property(x => x.Formula).HasColumnName("Formula");
            builder.Property(x => x.AvatarPicture).HasColumnName("AvatarPicture");
            builder.Property(x => x.Note).HasColumnName("Note");
            builder.Property(x => x.MenuGroupID).HasColumnName("MenuGroupID").IsRequired();
            builder.Property(x => x.StatusID).HasColumnName("StatusID").IsRequired();
            builder.Property(x => x.UnitID).HasColumnName("UnitID").IsRequired();
            builder.Property(x => x.TasteQTy).HasColumnName("TasteQTy");
            builder.Property(x => x.PrintSticker).HasColumnName("PrintSticker");

            // Relationships
            builder.HasOne<MenuGroup>(m => m.MenuGroup)
                .WithMany()
                .HasForeignKey(m => m.MenuGroupID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Unit>(m => m.Unit)
                .WithMany()
                .HasForeignKey(m => m.UnitID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Status>(m => m.Status)
                .WithMany()
                .HasForeignKey(m => m.StatusID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
