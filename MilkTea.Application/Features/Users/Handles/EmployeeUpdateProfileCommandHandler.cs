using MilkTea.Application.Features.Users.Commands;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Shared.Domain.Constants;
using Shared.Abstractions.CQRS;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

namespace MilkTea.Application.Features.Users.Handles;

public sealed class EmployeeUpdateProfileCommandHandler(
    IUserUnitOfWork userUnitOfWork,
    ICurrentUser currentUser) : ICommandHandler<EmployeeUpdateProfileCommand, EmployeeUpdateProfileResult>
{
    private readonly IUserUnitOfWork _vUserUnitOfWork = userUnitOfWork;
    private readonly ICurrentUser _vCurrentUser = currentUser;
    public async Task<EmployeeUpdateProfileResult> Handle(EmployeeUpdateProfileCommand command, CancellationToken cancellationToken)
    {
        var result = new EmployeeUpdateProfileResult();
        var userId = _vCurrentUser.UserId;

        Console.WriteLine(command.ToString());

        var user = await _vUserUnitOfWork.Users.GetByIdAsync(userId, cancellationToken);
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
        // if having error validation, return error
        if (result.ResultData.HasData) return result;

        await _vUserUnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // Update all fields using UpdateProfile method
            var hasUpdate = employee.UpdateProfile(
                fullName: !string.IsNullOrWhiteSpace(command.FullName) ? command.FullName.Trim() : null,
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
