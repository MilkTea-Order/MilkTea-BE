using MediatR;
using MilkTea.Application.Features.Users.Commands;
using MilkTea.Application.Ports.Users;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Domain.SharedKernel.Repositories;
using MilkTea.Shared.Domain.Constants;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

namespace MilkTea.Application.Features.Users.Commands;

public sealed class EmployeeUpdateProfileCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IEmployeeRepository employeeRepository,
    IGenderRepository genderRepository,
    ICurrentUser currentUser) : IRequestHandler<EmployeeUpdateProfileCommand, EmployeeUpdateProfileResult>
{
    public async Task<EmployeeUpdateProfileResult> Handle(EmployeeUpdateProfileCommand command, CancellationToken cancellationToken)
    {
        var result = new EmployeeUpdateProfileResult();
        result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);
        var userId = currentUser.UserId;

        var user = await userRepository.GetByIdAsync(userId);
        if (user is null)
            return SendError(result, ErrorCode.E0001, "User");

        var employee = await employeeRepository.GetByIdAsync(user.EmployeesID);
        if (employee is null)
            return SendError(result, ErrorCode.E0001, "Employee");

        // Validate email uniqueness
        if (!string.IsNullOrWhiteSpace(command.Email) && command.Email != employee.Email)
        {
            var isEmailExist = await employeeRepository.IsEmailExistAsync(command.Email, employee.Id);
            if (isEmailExist)
                return SendError(result, ErrorCode.E0002, "Email");
        }

        // Validate phone uniqueness
        if (!string.IsNullOrWhiteSpace(command.CellPhone) && command.CellPhone != employee.CellPhone)
        {
            var isPhoneExist = await employeeRepository.IsCellPhoneExistAsync(command.CellPhone, employee.Id);
            if (isPhoneExist)
                return SendError(result, ErrorCode.E0002, "CellPhone");
        }

        await unitOfWork.BeginTransactionAsync();
        try
        {
            // Update fields
            if (!string.IsNullOrWhiteSpace(command.FullName))
                employee.FullName = command.FullName.Trim();

            if (!string.IsNullOrWhiteSpace(command.IdentityCode))
                employee.IdentityCode = command.IdentityCode.Trim();

            if (!string.IsNullOrWhiteSpace(command.Email))
                employee.Email = command.Email.Trim();

            if (!string.IsNullOrWhiteSpace(command.Address))
                employee.Address = command.Address.Trim();

            if (!string.IsNullOrWhiteSpace(command.CellPhone))
                employee.CellPhone = command.CellPhone.Trim();

            if (!string.IsNullOrWhiteSpace(command.BankName))
                employee.BankName = command.BankName.Trim();

            if (!string.IsNullOrWhiteSpace(command.BankAccountName))
                employee.BankAccountName = command.BankAccountName.Trim();

            if (!string.IsNullOrWhiteSpace(command.BankAccountNumber))
                employee.BankAccountNumber = command.BankAccountNumber.Trim();

            // Validate and update GenderID
            if (command.GenderID.HasValue)
            {
                if (command.GenderID <= 0)
                {
                    await unitOfWork.RollbackTransactionAsync();
                    return SendError(result, ErrorCode.E0036, nameof(command.GenderID));
                }

                var isExist = await genderRepository.ExistsGenderAsync(command.GenderID.Value);
                if (!isExist)
                {
                    await unitOfWork.RollbackTransactionAsync();
                    return SendError(result, ErrorCode.E0001, "GenderID");
                }

                employee.GenderID = command.GenderID.Value;
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

                employee.BirthDay = birthDay.ToString("dd/MM/yyyy");
            }

            // Validate and update BankQRCode
            if (command.BankQRCode != null)
            {
                if (command.BankQRCode.Length == 0 || command.BankQRCode.Length > 5 * 1024 * 1024)
                {
                    await unitOfWork.RollbackTransactionAsync();
                    return SendError(result, ErrorCode.E0036, nameof(command.BankQRCode));
                }

                try
                {
                    using var inputStream = command.BankQRCode.OpenReadStream();
                    using var image = Image.Load(inputStream);
                    using var pngStream = new MemoryStream();
                    image.Save(pngStream, new PngEncoder());
                    employee.BankQRCode = pngStream.ToArray();
                }
                catch
                {
                    await unitOfWork.RollbackTransactionAsync();
                    return SendError(result, ErrorCode.E0036, nameof(command.BankQRCode));
                }
            }

            // Update audit fields
            employee.LastUpdatedBy = userId;
            employee.LastUpdatedDate = DateTime.UtcNow;

            await employeeRepository.UpdateAsync(employee);
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
