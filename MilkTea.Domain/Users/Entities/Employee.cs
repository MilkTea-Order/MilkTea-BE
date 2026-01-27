using MilkTea.Domain.SharedKernel.Abstractions;
using MilkTea.Domain.Users.Enums;
using MilkTea.Domain.Users.ValueObject;

namespace MilkTea.Domain.Users.Entities;

public class Employee : Aggregate<int>
{
    public string? Code { get; private set; }
    public string FullName { get; private set; } = null!;
    public int GenderID { get; private set; }

    public BirthDay BirthDay { get; private set; } = BirthDay.Empty();
    public string? IdentityCode { get; private set; }

    public Email Email { get; private set; } = Email.Empty();
    public string? Address { get; private set; }

    public DateTime? StartWorkingDate { get; private set; }
    public DateTime? EndWorkingDate { get; private set; }
    public int PositionID { get; private set; }

    public new int? CreatedBy { get; private set; }
    public new DateTime? CreatedDate { get; private set; }

    public int? LastUpdatedBy { get; private set; }
    public DateTime? LastUpdatedDate { get; private set; }

    public UserStatus Status { get; private set; }

    public PhoneNumber CellPhone { get; private set; } = PhoneNumber.Empty();

    public int? SalaryByHour { get; private set; }
    public DateTime? ShiftFrom { get; private set; }
    public DateTime? ShiftTo { get; private set; }
    public int? CalcSalaryByMinutes { get; private set; }

    public int TimekeepingOther { get; private set; }

    public BankAccount BankAccount { get; private set; } = BankAccount.Empty();

    public bool? IsBreakTime { get; private set; }
    public DateTime? BreakTimeFrom { get; private set; }
    public DateTime? BreakTimeTo { get; private set; }

    public bool IsActive => Status == UserStatus.Active;

    public Gender? Gender { get; set; }
    public Position? Position { get; set; }

    private Employee() { }

    public static Employee Create(
        string fullName,
        int genderId,
        int positionId,
        int createdBy,
        string? code = null,
        string? identityCode = null,
        string? address = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(genderId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(positionId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        var now = DateTime.UtcNow;

        return new Employee
        {
            FullName = fullName.Trim(),
            GenderID = genderId,
            PositionID = positionId,
            Code = code?.Trim(),
            IdentityCode = identityCode?.Trim(),
            Address = address?.Trim(),
            Status = UserStatus.Active,
            CreatedBy = createdBy,
            CreatedDate = now,
            BirthDay = BirthDay.Empty(),
            Email = Email.Empty(),
            CellPhone = PhoneNumber.Empty(),
            BankAccount = BankAccount.Empty()
        };
    }

    public bool UpdateFullName(string fullName, int updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        if (UpdateFullNameInternal(fullName))
        {
            Touch(updatedBy);
            return true;
        }
        return false;
    }

    private bool UpdateFullNameInternal(string fullName)
    {
        FullName = fullName.Trim();
        return true;
    }

    public bool UpdateEmail(string? email, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);
        if (UpdateEmailInternal(email))
        {
            Touch(updatedBy);
            return true;
        }
        return false;
    }

    private bool UpdateEmailInternal(string? email)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            Email = Email.Of(email);
            return true;
        }
        return false;
    }

    public bool UpdateBirthDay(string? birthDay, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);
        if (UpdateBirthDayInternal(birthDay))
        {
            Touch(updatedBy);
            return true;
        }
        return false;
    }

    private bool UpdateBirthDayInternal(string? birthDay)
    {
        if (!string.IsNullOrWhiteSpace(birthDay))
        {
            BirthDay = BirthDay.Of(birthDay);
            return true;
        }
        return false;
    }

    public bool UpdateCellPhone(string? cellPhone, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);
        if (UpdateCellPhoneInternal(cellPhone))
        {
            Touch(updatedBy);
            return true;
        }
        return false;
    }

    private bool UpdateCellPhoneInternal(string? cellPhone)
    {
        if (!string.IsNullOrWhiteSpace(cellPhone))
        {
            CellPhone = PhoneNumber.Of(cellPhone);
            return true;
        }
        return false;
    }

    public bool UpdateBankAccount(
        string? accountNumber,
        string? accountName,
        string? bankName,
        byte[]? qrCode,
        int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);
        if (UpdateBankAccountInternal(accountNumber, accountName, bankName, qrCode))
        {
            Touch(updatedBy);
            return true;
        }
        return false;
    }

    private bool UpdateBankAccountInternal(
        string? accountNumber,
        string? accountName,
        string? bankName,
        byte[]? qrCode)
    {
        if (!string.IsNullOrWhiteSpace(accountNumber) ||
            !string.IsNullOrWhiteSpace(accountName) ||
            !string.IsNullOrWhiteSpace(bankName) ||
            qrCode is not null)
        {
            BankAccount = BankAccount.UpdatePartial(accountNumber, accountName, bankName, qrCode);
            return true;
        }
        return false;
    }

    public bool UpdateIdentityCode(string? identityCode, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);
        if (UpdateIdentityCodeInternal(identityCode))
        {
            Touch(updatedBy);
            return true;
        }
        return false;
    }

    private bool UpdateIdentityCodeInternal(string? identityCode)
    {
        if (!string.IsNullOrWhiteSpace(identityCode))
        {
            IdentityCode = identityCode.Trim();
            return true;
        }
        return false;
    }

    public bool UpdateAddress(string? address, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);
        if (UpdateAddressInternal(address))
        {
            Touch(updatedBy);
            return true;
        }
        return false;
    }

    private bool UpdateAddressInternal(string? address)
    {
        if (!string.IsNullOrWhiteSpace(address))
        {
            Address = address.Trim();
            return true;
        }
        return false;
    }

    public bool UpdateGender(int genderId, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(genderId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);
        if (UpdateGenderInternal(genderId))
        {
            Touch(updatedBy);
            return true;
        }
        return false;
    }

    private bool UpdateGenderInternal(int genderId)
    {
        GenderID = genderId;
        return true;
    }

    public bool UpdatePosition(int positionId, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(positionId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        PositionID = positionId;
        Touch(updatedBy);
        return true;
    }

    public void Deactivate(int stoppedBy)
    {
        if (Status == UserStatus.Inactive)
            throw new InvalidOperationException("Employee is already inactive.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(stoppedBy);

        Status = UserStatus.Inactive;
        EndWorkingDate = DateTime.UtcNow;
        Touch(stoppedBy);
    }

    public void Activate(int activatedBy)
    {
        if (Status == UserStatus.Active)
            throw new InvalidOperationException("Employee is already active.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(activatedBy);

        Status = UserStatus.Active;
        EndWorkingDate = null;
        Touch(activatedBy);
    }

    public void UpdateWorkingDates(DateTime? startWorkingDate, DateTime? endWorkingDate, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        StartWorkingDate = startWorkingDate;
        EndWorkingDate = endWorkingDate;
        Touch(updatedBy);
    }

    public void UpdateSalary(int? salaryByHour, int? calcSalaryByMinutes, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        SalaryByHour = salaryByHour;
        CalcSalaryByMinutes = calcSalaryByMinutes;
        Touch(updatedBy);
    }

    public void UpdateShift(DateTime? shiftFrom, DateTime? shiftTo, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        ShiftFrom = shiftFrom;
        ShiftTo = shiftTo;
        Touch(updatedBy);
    }

    public void UpdateBreakTime(bool? isBreakTime, DateTime? breakTimeFrom, DateTime? breakTimeTo, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        IsBreakTime = isBreakTime;
        BreakTimeFrom = breakTimeFrom;
        BreakTimeTo = breakTimeTo;
        Touch(updatedBy);
    }

    /// <summary>
    /// Updates multiple profile fields at once.
    /// Passing null or empty string means "do not change" - keeps the existing value.
    /// Returns true if at least one field was updated.
    /// </summary>
    public bool UpdateProfile(
        string? fullName = null,
        int? genderId = null,
        string? birthDay = null,
        string? identityCode = null,
        string? email = null,
        string? address = null,
        string? cellPhone = null,
        string? bankAccountNumber = null,
        string? bankAccountName = null,
        string? bankName = null,
        byte[]? bankQRCode = null,
        int updatedBy = 0)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        bool hasUpdate = false;

        // Tái sử dụng các hàm update internal (không gọi Touch)
        if (!string.IsNullOrWhiteSpace(fullName))
        {
            if (UpdateFullNameInternal(fullName))
                hasUpdate = true;
        }

        if (genderId.HasValue && genderId.Value > 0)
        {
            if (UpdateGenderInternal(genderId.Value))
                hasUpdate = true;
        }

        if (UpdateBirthDayInternal(birthDay))
            hasUpdate = true;

        if (UpdateEmailInternal(email))
            hasUpdate = true;

        if (UpdateCellPhoneInternal(cellPhone))
            hasUpdate = true;

        if (UpdateIdentityCodeInternal(identityCode))
            hasUpdate = true;

        if (UpdateAddressInternal(address))
            hasUpdate = true;

        if (UpdateBankAccountInternal(bankAccountNumber, bankAccountName, bankName, bankQRCode))
            hasUpdate = true;

        if (hasUpdate)
        {
            Touch(updatedBy);
        }

        return hasUpdate;
    }

    private void Touch(int updatedBy)
    {
        LastUpdatedBy = updatedBy;
        LastUpdatedDate = DateTime.UtcNow;
    }
}
