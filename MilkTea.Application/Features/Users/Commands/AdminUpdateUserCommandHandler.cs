using MediatR;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.Users.Enums;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.Features.Users.Commands;

public sealed class AdminUpdateUserCommandHandler(
    IUserUnitOfWork userUnitOfWork,
    ICurrentUser currentUser) : IRequestHandler<AdminUpdateUserCommand, AdminUpdateUserResult>
{
    public async Task<AdminUpdateUserResult> Handle(AdminUpdateUserCommand command, CancellationToken cancellationToken)
    {
        var result = new AdminUpdateUserResult();
        result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

        if (command.UserID <= 0)
            return SendError(result, ErrorCode.E0036, "UserID");

        // Load entities with tracking for updates
        var user = await userUnitOfWork.Users.GetByIdForUpdateAsync(command.UserID, cancellationToken);
        if (user is null)
            return SendError(result, ErrorCode.E0001, "User");

        var employee = await userUnitOfWork.Employees.GetByIdForUpdateAsync(user.EmployeeID, cancellationToken);
        if (employee is null)
            return SendError(result, ErrorCode.E0001, "Employee");

        await userUnitOfWork.BeginTransactionAsync();
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

            // Update employee fields using domain methods
            if (!string.IsNullOrWhiteSpace(command.FullName))
                employee.UpdateFullName(command.FullName, updatedBy);

            if (command.GenderID.HasValue)
                employee.UpdateGender(command.GenderID.Value, updatedBy);

            if (!string.IsNullOrWhiteSpace(command.BirthDay))
                employee.UpdateBirthDay(command.BirthDay, updatedBy);

            if (!string.IsNullOrWhiteSpace(command.IdentityCode))
                employee.UpdateIdentityCode(command.IdentityCode, updatedBy);

            if (!string.IsNullOrWhiteSpace(command.Email))
                employee.UpdateEmail(command.Email, updatedBy);

            if (!string.IsNullOrWhiteSpace(command.Address))
                employee.UpdateAddress(command.Address, updatedBy);

            if (!string.IsNullOrWhiteSpace(command.CellPhone))
                employee.UpdateCellPhone(command.CellPhone, updatedBy);

            if (command.PositionID.HasValue)
                employee.UpdatePosition(command.PositionID.Value, updatedBy);

            if (command.StartWorkingDate.HasValue || command.EndWorkingDate.HasValue)
                employee.UpdateWorkingDates(command.StartWorkingDate, command.EndWorkingDate, updatedBy);

            if (command.SalaryByHour.HasValue || command.CalcSalaryByMinutes.HasValue)
                employee.UpdateSalary(command.SalaryByHour, command.CalcSalaryByMinutes, updatedBy);

            if (command.ShiftFrom.HasValue || command.ShiftTo.HasValue)
                employee.UpdateShift(command.ShiftFrom, command.ShiftTo, updatedBy);

            if (command.IsBreakTime.HasValue || command.BreakTimeFrom.HasValue || command.BreakTimeTo.HasValue)
                employee.UpdateBreakTime(command.IsBreakTime, command.BreakTimeFrom, command.BreakTimeTo, updatedBy);

            // Update BankAccount if any field is provided
            if (!string.IsNullOrWhiteSpace(command.BankName) ||
                !string.IsNullOrWhiteSpace(command.BankAccountName) ||
                !string.IsNullOrWhiteSpace(command.BankAccountNumber) ||
                command.BankQRCode != null)
            {
                employee.UpdateBankAccount(
                    command.BankAccountNumber,
                    command.BankAccountName,
                    command.BankName,
                    command.BankQRCode,
                    updatedBy);
            }

            // Commit transaction (SaveChangesAsync is called inside CommitTransactionAsync)
            await userUnitOfWork.CommitTransactionAsync();

            return result;
        }
        catch (Exception)
        {
            await userUnitOfWork.RollbackTransactionAsync();
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
