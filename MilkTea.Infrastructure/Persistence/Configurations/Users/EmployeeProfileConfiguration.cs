using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Users.Entities;
using MilkTea.Domain.Users.Enums;

namespace MilkTea.Infrastructure.Persistence.Configurations.Identity;

public class EmployeeProfileConfiguration : IEntityTypeConfiguration<EmployeeProfile>
{
    public void Configure(EntityTypeBuilder<EmployeeProfile> builder)
    {
        builder.ToTable("employees");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");

        builder.Property(x => x.Code).HasColumnName("Code");
        builder.Property(x => x.FullName).HasColumnName("FullName").IsRequired();
        builder.Property(x => x.GenderID).HasColumnName("GenderID").IsRequired();
        builder.Property(x => x.BirthDay).HasColumnName("BirthDay");
        builder.Property(x => x.IdentityCode).HasColumnName("IdentityCode");
        builder.Property(x => x.Email).HasColumnName("Email");
        builder.Property(x => x.Address).HasColumnName("Address");
        builder.Property(x => x.StartWorkingDate).HasColumnName("StartWorkingDate");
        builder.Property(x => x.EndWorkingDate).HasColumnName("EndWorkingDate");
        builder.Property(x => x.PositionID).HasColumnName("PositionID").IsRequired();
        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(x => x.LastUpdatedBy).HasColumnName("LastUpdatedBy");
        builder.Property(x => x.LastUpdatedDate).HasColumnName("LastUpdatedDate");
        
        // Map enum to existing StatusID column
        builder.Property(x => x.Status)
            .HasColumnName("StatusID")
            .HasConversion<int>()
            .IsRequired();
        
        builder.Property(x => x.CellPhone).HasColumnName("CellPhone");
        builder.Property(x => x.SalaryByHour).HasColumnName("SalaryByHour");
        builder.Property(x => x.ShiftFrom).HasColumnName("ShiftFrom");
        builder.Property(x => x.ShiftTo).HasColumnName("ShiftTo");
        builder.Property(x => x.CalcSalaryByMinutes).HasColumnName("CalcSalaryByMinutes");
        builder.Property(x => x.TimekeepingOther).HasColumnName("TimekeepingOther");
        builder.Property(x => x.BankAccountNumber).HasColumnName("BankAccountNumber");
        builder.Property(x => x.BankAccountName).HasColumnName("BankAccountName");
        builder.Property(x => x.BankQRCode).HasColumnName("BankQRCode");
        builder.Property(x => x.BankName).HasColumnName("BankName");
        builder.Property(x => x.IsBreakTime).HasColumnName("IsBreakTime");
        builder.Property(x => x.BreakTimeFrom).HasColumnName("BreakTimeFrom");
        builder.Property(x => x.BreakTimeTo).HasColumnName("BreakTimeTo");

        // Relationships
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
