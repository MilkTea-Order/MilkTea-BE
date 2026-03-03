using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Orders.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Order;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemEntity>
{
    public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
    {
        builder.ToTable("OrdersDetail");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.OrderId)
            .HasColumnName("OrderID")
            .IsRequired();

        builder.Property(x => x.Quantity).HasColumnName("Quantity").IsRequired();
        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(x => x.CancelledBy).HasColumnName("CancelledBy");
        builder.Property(x => x.CancelledDate).HasColumnName("CancelledDate");
        builder.Property(x => x.Note).HasColumnName("Note");

        builder.Ignore(x => x.UpdatedBy);
        builder.Ignore(x => x.UpdatedDate);
        builder.Ignore(x => x.TotalAmount);

        builder.ComplexProperty(x => x.MenuItem, menuItem =>
        {
            menuItem.Property(m => m.MenuId).HasColumnName("MenuID").IsRequired();
            menuItem.Property(m => m.SizeId).HasColumnName("SizeID").IsRequired();
            menuItem.Property(m => m.PriceListId).HasColumnName("PriceListID");

            menuItem.Property(m => m.Price)
                .HasColumnName("Price")
                .HasColumnType("decimal(10,0)")
                .IsRequired();

            menuItem.Property(m => m.KindOfHotpot1Id).HasColumnName("KindOfHotpot1ID");
            menuItem.Property(m => m.KindOfHotpot2Id).HasColumnName("KindOfHotpot2ID");
        });

        //builder.Navigation("Order");
    }
}
