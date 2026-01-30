using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MilkTea.Application.Features.Users.Commands;

public class EmployeeUpdateProfileCommand : ICommand<EmployeeUpdateProfileResult>
{
    public string? FullName { get; set; } = null!;
    public int? GenderID { get; set; } = null!;
    public string? BirthDay { get; set; } = null!;
    public string? IdentityCode { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public string? Address { get; set; } = null!;
    public string? CellPhone { get; set; } = null!;
    public string? BankName { get; set; } = null!;
    public string? BankAccountName { get; set; } = null!;
    public string? BankAccountNumber { get; set; } = null!;
    public IFormFile? BankQRCode { get; set; } = null!;
}

public sealed class EmployeeUpdateProfileCommandValidator : AbstractValidator<EmployeeUpdateProfileCommand>
{
    public EmployeeUpdateProfileCommandValidator()
    {
        RuleFor(x => x.FullName)
            .Must(name =>
                string.IsNullOrWhiteSpace(name) ||
                name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length >= 2)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("fullName");

        RuleFor(x => x.GenderID)
            .Must(v => !v.HasValue || v.Value > 0)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("genderId");

        RuleFor(x => x.Email)
            .Must(email => string.IsNullOrWhiteSpace(email) || IsValidEmail(email))
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("email");

        RuleFor(x => x.BirthDay)
            .Must(IsValidBirthDay)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("birthDay");

        RuleFor(x => x.CellPhone)
            .Must(phone => string.IsNullOrWhiteSpace(phone) || RegexCellPhone(phone))
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("cellphone");

        RuleFor(x => x.IdentityCode)
            .Must(code => string.IsNullOrWhiteSpace(code) || RegexIdentityCode(code))
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("identityCode");

        RuleFor(x => x.BankAccountNumber)
            .Must(no => string.IsNullOrWhiteSpace(no) || RegexBankAccountNumber(no))
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("bankAccountNumber");

        RuleFor(x => x.BankQRCode)
            .Must(IsValidBankQrFile)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("bankQRCode");
        // If any bank info is provided -> require BankName + BankAccountName + BankAccountNumber (non-empty)
        RuleFor(x => x)
            .Custom((cmd, context) =>
            {
                var hasAnyBankInfo =
                    cmd.BankName != null ||
                    cmd.BankAccountName != null ||
                    cmd.BankAccountNumber != null ||
                    cmd.BankQRCode != null;

                if (!hasAnyBankInfo) return;

                if (string.IsNullOrWhiteSpace(cmd.BankName))
                    context.AddFailure(MakeFailure("bankName", ErrorCode.E0001));

                if (string.IsNullOrWhiteSpace(cmd.BankAccountName))
                    context.AddFailure(MakeFailure("bankAccountName", ErrorCode.E0001));

                if (string.IsNullOrWhiteSpace(cmd.BankAccountNumber))
                    context.AddFailure(MakeFailure("bankAccountNumber", ErrorCode.E0001));
            });
    }

    private static ValidationFailure MakeFailure(string propertyName, string errorCode)
        => new(propertyName, errorCode) { ErrorCode = errorCode };

    private static bool IsValidEmail(string email) => Regex.IsMatch(email.Trim(), @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

    private static bool RegexCellPhone(string phone) => Regex.IsMatch(phone.Trim(), @"^\+?[0-9]{8,15}$");

    private static bool RegexIdentityCode(string code) => Regex.IsMatch(code.Trim(), @"^(\d{9}|\d{12})$");

    private static bool RegexBankAccountNumber(string no) => Regex.IsMatch(no.Trim(), @"^[0-9]{6,20}$");

    private static bool IsValidBirthDay(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return true;

        value = value.Trim();

        if (!DateTime.TryParseExact(
                value,
                Domain.Users.ValueObject.BirthDay.Format, // "dd/MM/yyyy"
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var birthDate))
        {
            return false;
        }

        var today = DateTime.UtcNow.Date;
        if (birthDate.Date > today) return false;

        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        // 10 -100 age
        return age >= Domain.Users.ValueObject.BirthDay.MinAge
               && age <= Domain.Users.ValueObject.BirthDay.MaxAge;
    }

    private static bool IsValidBankQrFile(IFormFile? file)
    {
        if (file is null) return true;

        if (file.Length <= 0 || file.Length > 5 * 1024 * 1024)
            return false;

        var permittedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        return permittedExtensions.Contains(extension);
    }
}
