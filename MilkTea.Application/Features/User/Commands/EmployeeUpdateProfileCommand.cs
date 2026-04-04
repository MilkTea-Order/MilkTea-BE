using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using MilkTea.Application.Features.Auth.Abstractions.Services;
using MilkTea.Application.Features.User.Model.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Common.Constants;
using MilkTea.Domain.User.Repositories;
using MilkTea.Shared.Domain.Constants;
using Shared.Abstractions.CQRS;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MilkTea.Application.Features.User.Commands;

public class EmployeeUpdateProfileCommand : ICommand<EmployeeUpdateProfileResult>
{
    public string? FullName { get; set; } = null!;

    public IFormFile? Avatar { get; set; } = null!;
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

        RuleFor(x => x.Avatar)
            .Must(IsValidAvatarFile)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName("avatar");

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

        var today = DateTime.Today;
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
        {
            return false;
        }

        var permittedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        return permittedExtensions.Contains(extension);
    }

    private static bool IsValidAvatarFile(IFormFile? file)
    {
        if (file is null) return true;
        if (file.Length <= 0 || file.Length > 5 * 1024 * 1024)
        {
            return false;
        }
        var permittedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return permittedExtensions.Contains(extension);
    }
}

public sealed class EmployeeUpdateProfileCommandHandler(IUserUnitOfWork userUnitOfWork,
                                                         IAuthService authServices,
                                                         IIdentifyServicePorts currentUser) : ICommandHandler<EmployeeUpdateProfileCommand, EmployeeUpdateProfileResult>
{
    private readonly IUserUnitOfWork _vUserUnitOfWork = userUnitOfWork;
    private readonly IAuthService _vAuthServices = authServices;
    private readonly IIdentifyServicePorts _vCurrentUser = currentUser;
    public async Task<EmployeeUpdateProfileResult> Handle(EmployeeUpdateProfileCommand command, CancellationToken cancellationToken)
    {
        var result = new EmployeeUpdateProfileResult();
        var userId = _vCurrentUser.UserId;


        var user = await _vAuthServices.GetAccountByUserIdAsync(userId, cancellationToken);
        // No Authorization => logout
        if (user is null)
        {
            result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            return result;
        }

        // No Authorization => logout
        var employee = await _vUserUnitOfWork.Employees.GetByIdForUpdateAsync(user.EmployeeID, cancellationToken);
        if (employee is null)
        {
            result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            return result;
        }

        // Validate email duplicate
        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            var currentEmail = employee.Email?.Value;
            if (command.Email != currentEmail)
            {
                var isEmailExist = await _vUserUnitOfWork.Employees.IsEmailExistAsync(command.Email, employee.Id, cancellationToken);
                if (isEmailExist) result = SendError(result, ErrorCode.E0002, "email");
            }
        }

        // Validate phone duplicate
        if (!string.IsNullOrWhiteSpace(command.CellPhone))
        {
            var currentPhone = employee.CellPhone?.Value;
            if (command.CellPhone != currentPhone)
            {
                var isPhoneExist = await _vUserUnitOfWork.Employees.IsCellPhoneExistAsync(command.CellPhone, employee.Id, cancellationToken);
                if (isPhoneExist) result = SendError(result, ErrorCode.E0002, "cellphone");
            }
        }


        byte[]? qrCodeBytes = null;
        if (command.BankQRCode != null)
        {
            try
            {
                using var inputStream = command.BankQRCode.OpenReadStream();
                using var image = Image.Load(inputStream);
                using var pngStream = new MemoryStream();
                image.Save(pngStream, new PngEncoder());
                qrCodeBytes = pngStream.ToArray();
            }
            catch
            {
                return SendError(result, ErrorCode.E0036, "bankQRCode");
            }
        }

        byte[]? avatar = null;
        if (command.Avatar != null)
        {
            try
            {
                using var inputStream = command.Avatar.OpenReadStream();
                using var image = Image.Load(inputStream);
                using var pngStream = new MemoryStream();
                image.Save(pngStream, new PngEncoder());
                avatar = pngStream.ToArray();
            }
            catch
            {
                return SendError(result, ErrorCode.E0036, "avatar");
            }
        }

        // if having error validation, return error
        if (result.ResultData.HasData) return result;

        await _vUserUnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // Update all fields using UpdateProfile method
            var hasUpdate = employee.UpdateProfile(
                fullName: !string.IsNullOrWhiteSpace(command.FullName) ? command.FullName.Trim() : null,
                avatar: avatar,
                genderId: command.GenderID,
                birthDay: !string.IsNullOrWhiteSpace(command.BirthDay) ? command.BirthDay.Trim() : null,
                identityCode: !string.IsNullOrWhiteSpace(command.IdentityCode) ? command.IdentityCode.Trim() : null,
                email: !string.IsNullOrWhiteSpace(command.Email) ? command.Email.Trim() : null,
                address: !string.IsNullOrWhiteSpace(command.Address) ? command.Address.Trim() : null,
                cellPhone: !string.IsNullOrWhiteSpace(command.CellPhone) ? command.CellPhone.Trim() : null,
                bankAccountNumber: !string.IsNullOrWhiteSpace(command.BankAccountNumber) ? command.BankAccountNumber.Trim() : null,
                bankAccountName: !string.IsNullOrWhiteSpace(command.BankAccountName) ? command.BankAccountName.Trim() : null,
                bankName: !string.IsNullOrWhiteSpace(command.BankName) ? command.BankName.Trim() : null,
                bankQRCode: qrCodeBytes,
                updatedBy: userId);

            // Commit transaction 
            await _vUserUnitOfWork.CommitTransactionAsync(cancellationToken);
            return result;
        }
        catch (ArgumentException ex)
        {
            var errorCode = !string.IsNullOrWhiteSpace(ex.Message) ? ex.Message : ErrorCode.E0036;
            var field = ex.ParamName;

            if (!string.IsNullOrWhiteSpace(field))
            {
                if (errorCode == ErrorCode.E0001)
                {
                    foreach (var item in field.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        result.ResultData.Add(errorCode, item.Trim());
                    }
                }
                else
                {
                    result.ResultData.Add(errorCode, field.Trim());
                }
            }
            else
            {
                result.ResultData.Add(errorCode, "employeeUpdateProfile");
            }
            await _vUserUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return result;
        }
        catch (Exception)
        {
            await _vUserUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E9999, "employeeUpdateProfile");
        }
    }
    private static EmployeeUpdateProfileResult SendError(EmployeeUpdateProfileResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
