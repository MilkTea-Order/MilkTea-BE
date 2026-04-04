using FluentValidation;
using MediatR;
using MilkTea.Application.Features.User.Model.Results;

namespace MilkTea.Application.Features.User.Commands;

public class AdminUpdateUserCommand : IRequest<AdminUpdateUserResult>
{
    public int UserID { get; set; }

    public string? FullName { get; set; }
    public int? GenderID { get; set; }
    public string? BirthDay { get; set; }
    public string? IdentityCode { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? CellPhone { get; set; }


    public int? PositionID { get; set; }
    public DateTime? StartWorkingDate { get; set; }
    public DateTime? EndWorkingDate { get; set; }
    public int? StatusID { get; set; }


    public int? SalaryByHour { get; set; }
    public int? CalcSalaryByMinutes { get; set; }


    public DateTime? ShiftFrom { get; set; }
    public DateTime? ShiftTo { get; set; }
    public bool? IsBreakTime { get; set; }
    public DateTime? BreakTimeFrom { get; set; }
    public DateTime? BreakTimeTo { get; set; }


    public string? BankName { get; set; }
    public string? BankAccountName { get; set; }
    public string? BankAccountNumber { get; set; }
    public byte[]? BankQRCode { get; set; }
}

public sealed class AdminUpdateUserCommandValidator : AbstractValidator<AdminUpdateUserCommand>
{
    public AdminUpdateUserCommandValidator()
    {
        RuleFor(x => x.UserID)
            .GreaterThan(0)
            .WithMessage("UserID phải lớn hơn 0");
    }
}

//public sealed class AdminUpdateUserCommandHandler(IUserUnitOfWork userUnitOfWork,
//                                                    IIdentifyServicePorts currentUser) : IRequestHandler<AdminUpdateUserCommand, AdminUpdateUserResult>
//{
//    public async Task<AdminUpdateUserResult> Handle(AdminUpdateUserCommand command, CancellationToken cancellationToken)
//    {
//        var result = new AdminUpdateUserResult();
//        result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.Now);

//        if (command.UserID <= 0)
//        {
//            return SendError(result, ErrorCode.E0036, nameof(command.UserID));
//        }

//        var user = await userUnitOfWork.Users.GetByIdForUpdateAsync(command.UserID, cancellationToken);
//        if (user is null)
//            return SendError(result, ErrorCode.E0001, nameof(command.UserID));

//        var employee = await userUnitOfWork.Employees.GetByIdForUpdateAsync(user.EmployeeID, cancellationToken);
//        if (employee is null)
//            return SendError(result, ErrorCode.E0001, nameof(user.EmployeeID));
//        await userUnitOfWork.BeginTransactionAsync();
//        try
//        {
//            var updatedBy = currentUser.UserId;

//            // Update user status if provided
//            if (command.StatusID.HasValue)
//            {
//                if (!Enum.IsDefined(typeof(UserStatus), command.StatusID.Value))
//                    return SendError(result, ErrorCode.E0001, "StatusID");

//                var newStatus = (UserStatus)command.StatusID.Value;
//                if (newStatus == UserStatus.Active)
//                    user.Activate(updatedBy);
//                else if (newStatus == UserStatus.Inactive)
//                    user.Deactivate(updatedBy);
//            }

//            // Update employee fields using domain methods
//            if (!string.IsNullOrWhiteSpace(command.FullName))
//                employee.UpdateFullName(command.FullName, updatedBy);

//            if (command.GenderID.HasValue)
//                employee.UpdateGender(command.GenderID.Value, updatedBy);

//            if (!string.IsNullOrWhiteSpace(command.BirthDay))
//                employee.UpdateBirthDay(command.BirthDay, updatedBy);

//            if (!string.IsNullOrWhiteSpace(command.IdentityCode))
//                employee.UpdateIdentityCode(command.IdentityCode, updatedBy);

//            if (!string.IsNullOrWhiteSpace(command.Email))
//                employee.UpdateEmail(command.Email, updatedBy);

//            if (!string.IsNullOrWhiteSpace(command.Address))
//                employee.UpdateAddress(command.Address, updatedBy);

//            if (!string.IsNullOrWhiteSpace(command.CellPhone))
//                employee.UpdateCellPhone(command.CellPhone, updatedBy);

//            if (command.PositionID.HasValue)
//                employee.UpdatePosition(command.PositionID.Value, updatedBy);

//            if (command.StartWorkingDate.HasValue || command.EndWorkingDate.HasValue)
//                employee.UpdateWorkingDates(command.StartWorkingDate, command.EndWorkingDate, updatedBy);

//            if (command.SalaryByHour.HasValue || command.CalcSalaryByMinutes.HasValue)
//                employee.UpdateSalary(command.SalaryByHour, command.CalcSalaryByMinutes, updatedBy);

//            if (command.ShiftFrom.HasValue || command.ShiftTo.HasValue)
//                employee.UpdateShift(command.ShiftFrom, command.ShiftTo, updatedBy);

//            if (command.IsBreakTime.HasValue || command.BreakTimeFrom.HasValue || command.BreakTimeTo.HasValue)
//                employee.UpdateBreakTime(command.IsBreakTime, command.BreakTimeFrom, command.BreakTimeTo, updatedBy);

//            // Update BankAccount if any field is provided
//            if (!string.IsNullOrWhiteSpace(command.BankName) ||
//                !string.IsNullOrWhiteSpace(command.BankAccountName) ||
//                !string.IsNullOrWhiteSpace(command.BankAccountNumber) ||
//                command.BankQRCode != null)
//            {
//                employee.UpdateBankAccount(
//                    command.BankAccountNumber,
//                    command.BankAccountName,
//                    command.BankName,
//                    command.BankQRCode,
//                    updatedBy);
//            }

//            // Commit transaction (SaveChangesAsync is called inside CommitTransactionAsync)
//            await userUnitOfWork.CommitTransactionAsync();

//            return result;
//        }
//        catch (Exception)
//        {
//            await userUnitOfWork.RollbackTransactionAsync();
//            return SendError(result, ErrorCode.E9999, "AdminUpdateUser");
//        }
//    }

//    private static AdminUpdateUserResult SendError(AdminUpdateUserResult result, string errorCode, params string[] values)
//    {
//        if (values is { Length: > 0 })
//            result.ResultData.Add(errorCode, values.ToList());
//        return result;
//    }
//}
