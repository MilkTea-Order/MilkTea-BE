using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MilkTea.Domain.Users.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    // Save: "" -> NULL
    // Load: NULL -> ""
    private static readonly ValueConverter<string, string?> EmptyToNull =
        new(
            // save
            v => string.IsNullOrEmpty(v) ? null : v,
            //load
            v => v ?? string.Empty
        );

    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");

        builder.Property(x => x.Code).HasColumnName("Code");
        builder.Property(x => x.FullName).HasColumnName("FullName").IsRequired();
        builder.Property(x => x.GenderID).HasColumnName("GenderID").IsRequired();

        builder.Property(x => x.IdentityCode).HasColumnName("IdentityCode");
        builder.Property(x => x.Address).HasColumnName("Address");

        builder.Property(x => x.StartWorkingDate).HasColumnName("StartWorkingDate");
        builder.Property(x => x.EndWorkingDate).HasColumnName("EndWorkingDate");
        builder.Property(x => x.PositionID).HasColumnName("PositionID").IsRequired();

        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(x => x.UpdatedBy).HasColumnName("LastUpdatedBy");
        builder.Property(x => x.UpdatedDate).HasColumnName("LastUpdatedDate");

        builder.Property(x => x.Status)
            .HasColumnName("StatusID")
            .HasConversion<int>()
            .IsRequired();

        builder.ComplexProperty(x => x.BirthDay, bd =>
        {
            bd.Property(p => p.Value)
              .HasColumnName("BirthDay")
              .HasConversion(EmptyToNull)
              .IsRequired(false);
        });

        builder.ComplexProperty(x => x.Email, e =>
        {
            e.Property(p => p.Value)
             .HasColumnName("Email")
             .HasConversion(EmptyToNull)
             .IsRequired(false);
        });

        builder.ComplexProperty(x => x.CellPhone, cp =>
        {
            cp.Property(p => p.Value)
              .HasColumnName("CellPhone")
              .HasConversion(EmptyToNull)
              .IsRequired(false);
        });

        builder.ComplexProperty(x => x.BankAccount, ba =>
        {
            ba.Property(p => p.AccountNumber)
              .HasColumnName("Bank_AccountNumber")
              .HasConversion(EmptyToNull)
              .IsRequired(false);

            ba.Property(p => p.AccountName)
              .HasColumnName("Bank_AccountName")
              .HasConversion(EmptyToNull)
              .IsRequired(false);

            ba.Property(p => p.BankName)
              .HasColumnName("BankName")
              .HasConversion(EmptyToNull)
              .IsRequired(false);

            ba.Property(p => p.QrCode)
              .HasColumnName("Bank_QRCode")
              .IsRequired(false);
        });

        builder.Property(x => x.SalaryByHour).HasColumnName("SalaryByHour");
        builder.Property(x => x.ShiftFrom).HasColumnName("ShiftFrom");
        builder.Property(x => x.ShiftTo).HasColumnName("ShiftTo");
        builder.Property(x => x.CalcSalaryByMinutes).HasColumnName("CalcSalaryByMinutes");
        builder.Property(x => x.TimekeepingOther).HasColumnName("TimekeepingOther");

        builder.Property(x => x.IsBreakTime).HasColumnName("IsBreakTime");
        builder.Property(x => x.BreakTimeFrom).HasColumnName("BreakTimeFrom");
        builder.Property(x => x.BreakTimeTo).HasColumnName("BreakTimeTo");

        builder.HasOne(e => e.Gender)
            .WithMany()
            .HasForeignKey(e => e.GenderID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Position)
            .WithMany()
            .HasForeignKey(e => e.PositionID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
