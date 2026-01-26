using MediatR;
using MilkTea.Application.Features.Users.Commands;
using MilkTea.Application.Ports.Users;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.Users.Enums;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Domain.SharedKernel.Repositories;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.Features.Users.Commands;

public sealed class AdminUpdateUserCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IEmployeeRepository employeeRepository,
    ICurrentUser currentUser) : IRequestHandler<AdminUpdateUserCommand, AdminUpdateUserResult>
{
    public async Task<AdminUpdateUserResult> Handle(AdminUpdateUserCommand command, CancellationToken cancellationToken)
    {
        var result = new AdminUpdateUserResult();
        result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

        if (command.UserID <= 0)
            return SendError(result, ErrorCode.E0036, "UserID");

        var user = await userRepository.GetByIdAsync(command.UserID);
        if (user is null)
            return SendError(result, ErrorCode.E0001, "User");

        var employee = await employeeRepository.GetByIdAsync(user.EmployeesID);
        if (employee is null)
            return SendError(result, ErrorCode.E0001, "Employee");

        await unitOfWork.BeginTransactionAsync();
        try
        {
            var updatedBy = currentUser.UserId;

            // Update user status if provided
            if (command.StatusID.HasValue)
            {
                if (!Enum.IsDefined(typeof(UserStatus), command.StatusID.Value))
                    return SendError(result, ErrorCode.E0001, "StatusID");

                var newStatus = (UserStatus)command.StatusID.Value;
                if (newStatus == UserStatus.Active)
                    user.Activate(updatedBy);
                else if (newStatus == UserStatus.Inactive)
                    user.Deactivate(updatedBy);
            }

            // Update employee fields
            if (!string.IsNullOrWhiteSpace(command.FullName))
                employee.FullName = command.FullName;

            if (command.GenderID.HasValue)
                employee.GenderID = command.GenderID.Value;

            if (!string.IsNullOrWhiteSpace(command.BirthDay))
                employee.BirthDay = command.BirthDay;

            if (!string.IsNullOrWhiteSpace(command.IdentityCode))
                employee.IdentityCode = command.IdentityCode;

            if (!string.IsNullOrWhiteSpace(command.Email))
                employee.Email = command.Email;

            if (!string.IsNullOrWhiteSpace(command.Address))
                employee.Address = command.Address;

            if (!string.IsNullOrWhiteSpace(command.CellPhone))
                employee.CellPhone = command.CellPhone;

            if (command.PositionID.HasValue)
                employee.PositionID = command.PositionID.Value;

            if (command.StartWorkingDate.HasValue)
                employee.StartWorkingDate = command.StartWorkingDate;

            if (command.EndWorkingDate.HasValue)
                employee.EndWorkingDate = command.EndWorkingDate;

            if (command.SalaryByHour.HasValue)
                employee.SalaryByHour = command.SalaryByHour;

            if (command.CalcSalaryByMinutes.HasValue)
                employee.CalcSalaryByMinutes = command.CalcSalaryByMinutes;

            if (command.ShiftFrom.HasValue)
                employee.ShiftFrom = command.ShiftFrom;

            if (command.ShiftTo.HasValue)
                employee.ShiftTo = command.ShiftTo;

            if (command.IsBreakTime.HasValue)
                employee.IsBreakTime = command.IsBreakTime;

            if (command.BreakTimeFrom.HasValue)
                employee.BreakTimeFrom = command.BreakTimeFrom;

            if (command.BreakTimeTo.HasValue)
                employee.BreakTimeTo = command.BreakTimeTo;

            if (!string.IsNullOrWhiteSpace(command.BankName))
                employee.BankName = command.BankName;

            if (!string.IsNullOrWhiteSpace(command.BankAccountName))
                employee.BankAccountName = command.BankAccountName;

            if (!string.IsNullOrWhiteSpace(command.BankAccountNumber))
                employee.BankAccountNumber = command.BankAccountNumber;

            if (command.BankQRCode != null)
                employee.BankQRCode = command.BankQRCode;

            // Update audit fields
            employee.LastUpdatedBy = updatedBy;
            employee.LastUpdatedDate = DateTime.UtcNow;

            await employeeRepository.UpdateAsync(employee);
            if (command.StatusID.HasValue)
            {
                await userRepository.UpdateAsync(user);
            }

            await unitOfWork.CommitTransactionAsync();

            return result;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync();
            return SendError(result, ErrorCode.E9999, "AdminUpdateUser");
        }
    }

    private static AdminUpdateUserResult SendError(AdminUpdateUserResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
