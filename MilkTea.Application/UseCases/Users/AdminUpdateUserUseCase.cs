using MilkTea.Application.Commands.Users;
using MilkTea.Application.Results.Users;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.UseCases.Users
{
    public class AdminUpdateUserUseCase(
                            IUserRepository userRepository,
                            IEmployeeRepository employeeRepository,
                            IStatusRepository statusRepository,
                            IUnitOfWork unitOfWork)
    {
        private readonly IUserRepository _vUserRepository = userRepository;
        private readonly IEmployeeRepository _vEmployeeRepository = employeeRepository;
        private readonly IStatusRepository _vStatusRepository = statusRepository;
        private readonly IUnitOfWork _vUnitOfWork = unitOfWork;

        public async Task<AdminUpdateUserResult> Execute(AdminUpdateUserCommand command)
        {
            AdminUpdateUserResult result = new();
            result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

            if (command.UserID <= 0) return SendMessageError(result, ErrorCode.E0036, "UserID");

            var user = await _vUserRepository.GetByIdAsync(command.UserID);
            if (user == null) return SendMessageError(result, ErrorCode.E0001, "User");

            var employee = await _vEmployeeRepository.GetByIdAsync(user.EmployeesID);
            if (employee == null) return SendMessageError(result, ErrorCode.E0001, "Employee");

            if (command.StatusID.HasValue)
            {
                if (!await _vStatusRepository.ExistsStatusAsync(command.StatusID.Value))
                    return SendMessageError(result, ErrorCode.E0001, "StatusID");

                user.StatusID = command.StatusID.Value;
            }

            if (command.FullName != null)
                employee.FullName = command.FullName;

            if (command.GenderID.HasValue)
                employee.GenderID = command.GenderID.Value;

            if (command.BirthDay != null)
                employee.BirthDay = command.BirthDay;

            if (command.IdentityCode != null)
                employee.IdentityCode = command.IdentityCode;

            if (command.Email != null)
                employee.Email = command.Email;

            if (command.Address != null)
                employee.Address = command.Address;

            if (command.CellPhone != null)
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

            if (command.BankName != null)
                employee.BankName = command.BankName;

            if (command.BankAccountName != null)
                employee.BankAccountName = command.BankAccountName;

            if (command.BankAccountNumber != null)
                employee.BankAccountNumber = command.BankAccountNumber;

            if (command.BankQRCode != null)
                employee.BankQRCode = command.BankQRCode;

            employee.LastUpdatedBy = command.AdminID;
            employee.LastUpdatedDate = DateTime.UtcNow;

            user.LastUpdatedBy = command.AdminID;
            user.LastUpdatedDate = DateTime.UtcNow;

            _vEmployeeRepository.Update(employee);
            await _vUnitOfWork.CommitAsync();

            return result;
        }
        private AdminUpdateUserResult SendMessageError(
            AdminUpdateUserResult result,
            string errorCode,
            params string[] values)
        {
            if (values != null && values.Length > 0)
                result.ResultData.Add(errorCode, values.ToList());

            return result;
        }
    }

}
