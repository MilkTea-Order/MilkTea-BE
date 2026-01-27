using MediatR;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.SharedKernel.Repositories;
using MilkTea.Shared.Domain.Constants;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

namespace MilkTea.Application.Features.Users.Commands;

public sealed class EmployeeUpdateProfileCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser) : IRequestHandler<EmployeeUpdateProfileCommand, EmployeeUpdateProfileResult>
{
    public async Task<EmployeeUpdateProfileResult> Handle(EmployeeUpdateProfileCommand command, CancellationToken cancellationToken)
    {
        var result = new EmployeeUpdateProfileResult();
        result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);
        var userId = currentUser.UserId;

        // Load entities (read-only for validation)
        var user = await unitOfWork.Users.GetByIdAsync(userId);
        if (user is null)
            return SendError(result, ErrorCode.E0001, "User");

        var employee = await unitOfWork.Employees.GetByIdAsync(user.EmployeeID);
        if (employee is null)
            return SendError(result, ErrorCode.E0001, "Employee");

        // Validate email uniqueness
        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            var currentEmail = employee.Email?.Value;
            if (command.Email != currentEmail)
            {
                var isEmailExist = await unitOfWork.Employees.IsEmailExistAsync(command.Email, employee.Id);
                if (isEmailExist)
                    return SendError(result, ErrorCode.E0002, "Email");
            }
        }

        // Validate phone uniqueness
        if (!string.IsNullOrWhiteSpace(command.CellPhone))
        {
            var currentPhone = employee.CellPhone?.Value;
            if (command.CellPhone != currentPhone)
            {
                var isPhoneExist = await unitOfWork.Employees.IsCellPhoneExistAsync(command.CellPhone, employee.Id);
                if (isPhoneExist)
                    return SendError(result, ErrorCode.E0002, "CellPhone");
            }
        }

        // Validate and prepare data before transaction
        byte[]? qrCodeBytes = null;
        if (command.BankQRCode != null)
        {
            if (command.BankQRCode.Length == 0 || command.BankQRCode.Length > 5 * 1024 * 1024)
                return SendError(result, ErrorCode.E0036, nameof(command.BankQRCode));

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
                return SendError(result, ErrorCode.E0036, nameof(command.BankQRCode));
            }
        }

        await unitOfWork.BeginTransactionAsync();
        try
        {
            // Load employee with tracking for update
            var employeeForUpdate = await unitOfWork.Employees.GetByIdForUpdateAsync(user.EmployeeID);
            if (employeeForUpdate is null)
            {
                await unitOfWork.RollbackTransactionAsync();
                return SendError(result, ErrorCode.E0001, "Employee");
            }

            // Update fields using domain methods
            if (!string.IsNullOrWhiteSpace(command.FullName))
                employeeForUpdate.UpdateFullName(command.FullName.Trim(), userId);

            if (!string.IsNullOrWhiteSpace(command.IdentityCode))
                employeeForUpdate.UpdateIdentityCode(command.IdentityCode.Trim(), userId);

            if (!string.IsNullOrWhiteSpace(command.Email))
                employeeForUpdate.UpdateEmail(command.Email.Trim(), userId);

            if (!string.IsNullOrWhiteSpace(command.Address))
                employeeForUpdate.UpdateAddress(command.Address.Trim(), userId);

            if (!string.IsNullOrWhiteSpace(command.CellPhone))
                employeeForUpdate.UpdateCellPhone(command.CellPhone.Trim(), userId);

            // Validate and update GenderID
            if (command.GenderID.HasValue)
            {
                if (command.GenderID <= 0)
                {
                    await unitOfWork.RollbackTransactionAsync();
                    return SendError(result, ErrorCode.E0036, nameof(command.GenderID));
                }

                var isExist = await unitOfWork.Genders.ExistsGenderAsync(command.GenderID.Value);
                if (!isExist)
                {
                    await unitOfWork.RollbackTransactionAsync();
                    return SendError(result, ErrorCode.E0001, "GenderID");
                }

                employeeForUpdate.UpdateGender(command.GenderID.Value, userId);
            }

            // Validate and update BirthDay
            if (command.BirthDay.HasValue)
            {
                var birthDay = command.BirthDay.Value.Date;
                var today = DateTime.UtcNow.Date;

                if (birthDay > today || birthDay < today.AddYears(-120) || birthDay > today.AddYears(-18))
                {
                    await unitOfWork.RollbackTransactionAsync();
                    return SendError(result, ErrorCode.E0036, nameof(command.BirthDay));
                }

                employeeForUpdate.UpdateBirthDay(birthDay.ToString("dd/MM/yyyy"), userId);
            }

            // Update BankAccount if any field is provided
            if (!string.IsNullOrWhiteSpace(command.BankName) ||
                !string.IsNullOrWhiteSpace(command.BankAccountName) ||
                !string.IsNullOrWhiteSpace(command.BankAccountNumber) ||
                qrCodeBytes != null)
            {
                employeeForUpdate.UpdateBankAccount(
                    command.BankAccountNumber?.Trim(),
                    command.BankAccountName?.Trim(),
                    command.BankName?.Trim(),
                    qrCodeBytes,
                    userId);
            }

            // Commit transaction (SaveChangesAsync is called inside CommitTransactionAsync)
            await unitOfWork.CommitTransactionAsync();

            return result;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync();
            return SendError(result, ErrorCode.E9999, "EmployeeUpdateProfile");
        }
    }

    private static EmployeeUpdateProfileResult SendError(EmployeeUpdateProfileResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
