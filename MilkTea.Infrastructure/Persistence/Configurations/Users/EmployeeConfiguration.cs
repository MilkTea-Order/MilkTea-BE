using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Users;

namespace MilkTea.Infrastructure.Persistence.Configurations.Users
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("employees");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

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
            builder.Property(x => x.StatusID).HasColumnName("StatusID").IsRequired();
            builder.Property(x => x.CellPhone).HasColumnName("CellPhone");
            builder.Property(x => x.SalaryByHour).HasColumnName("SalaryByHour");
            builder.Property(x => x.ShiftFrom).HasColumnName("ShiftFrom");
            builder.Property(x => x.ShiftTo).HasColumnName("ShiftTo");
            builder.Property(x => x.CalcSalaryByMinutes).HasColumnName("CalcSalaryByMinutes");
            builder.Property(x => x.TimekeepingOther).HasColumnName("TimekeepingOther").IsRequired();
            builder.Property(x => x.BankAccountNumber).HasColumnName("Bank_AccountNumber");
            builder.Property(x => x.BankAccountName).HasColumnName("Bank_AccountName");
            builder.Property(x => x.BankQRCode).HasColumnName("Bank_QRCode");
            builder.Property(x => x.BankName).HasColumnName("BankName");
            builder.Property(x => x.IsBreakTime).HasColumnName("IsBreakTime");
            builder.Property(x => x.BreakTimeFrom).HasColumnName("BreakTimeFrom");
            builder.Property(x => x.BreakTimeTo).HasColumnName("BreakTimeTo");

            // Relationships
            builder.HasOne<Gender>(e => e.Gender)
                .WithMany()
                .HasForeignKey(e => e.GenderID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Position>(e => e.Position)
                .WithMany()
                .HasForeignKey(e => e.PositionID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Status>(e => e.Status)
                .WithMany()
                .HasForeignKey(e => e.StatusID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
