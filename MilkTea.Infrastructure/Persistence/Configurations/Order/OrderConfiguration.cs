using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Orders.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Order;

public sealed class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.DinnerTableId).HasColumnName("DinnerTableID").IsRequired();

        builder.Property(x => x.OrderBy).HasColumnName("OrderBy").IsRequired();
        builder.Property(x => x.OrderDate).HasColumnName("OrderDate").IsRequired();

        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy");
        builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");

        builder.Property(x => x.CancelledBy).HasColumnName("CancelledBy");
        builder.Property(x => x.CancelledDate).HasColumnName("CancelledDate");

        builder.Property(x => x.Status)
            .HasColumnName("StatusOfOrderID")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Note)
            .HasColumnName("Note")
            .HasColumnType("longtext");

        builder.Property(x => x.PaymentedBy).HasColumnName("PaymentedBy");
        builder.Property(x => x.PaymentedDate).HasColumnName("PaymentedDate");
        builder.Property(x => x.PaymentedTotal).HasColumnName("PaymentedTotal");
        builder.Property(x => x.PaymentedType)
            .HasColumnName("PaymentedType")
            .HasMaxLength(10);

        builder.Property(x => x.AddNoteBy).HasColumnName("AddNoteBy");
        builder.Property(x => x.AddNoteDate).HasColumnName("AddNoteDate");

        builder.Property(x => x.ChangeBy).HasColumnName("ChangeBy");
        builder.Property(x => x.ChangeDate).HasColumnName("ChangeDate");

        builder.Property(x => x.MergedBy).HasColumnName("MergedBy");
        builder.Property(x => x.MergedDate).HasColumnName("MergedDate");

        //builder.ComplexProperty(x => x.BillNo, b =>
        //{
        //    b.Property(p => p!.Value)
        //        .HasColumnName("BillNo")
        //        .HasColumnType("tinytext")
        //        .IsRequired(false);
        //});

        builder.OwnsOne(x => x.BillNo, b =>
        {
            b.Property(p => p.Value)
                .HasColumnName("BillNo")
                .HasColumnType("tinytext")
                .IsRequired(false);
        });

        builder.Navigation(x => x.BillNo).IsRequired(false);

        builder.OwnsOne(x => x.Promotion, pb =>
        {
            pb.WithOwner();

            pb.Property(p => p.PromotionId)
                .HasColumnName("PromotionID");

            pb.Property(p => p.Percent)
                .HasColumnName("PromotionPercent")
                .IsRequired(false);

            pb.Property(p => p.Amount)
                .HasColumnName("PromotionAmount")
                .HasColumnType("decimal(10,0)")
                .IsRequired(false);
        });

        builder.Navigation(x => x.Promotion).IsRequired(false);

        builder.Property(x => x.TotalAmount)
            .HasColumnName("TotalAmount")
            .HasColumnType("decimal(10,0)");

        builder.Property(x => x.ActionBy).HasColumnName("ActionBy");
        builder.Property(x => x.ActionDate).HasColumnName("ActionDate");

        builder.HasMany(x => x.OrderItems)
                .WithOne()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.OrderItems)
                .HasField("_vOrderItems")
                .UsePropertyAccessMode(PropertyAccessMode.Field);


        builder.Ignore(x => x.DomainEvents);
    }
}
