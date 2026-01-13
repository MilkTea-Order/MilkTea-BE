using MilkTea.Application.Commands.Users;
using MilkTea.Application.Results.Users;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Shared.Domain.Constants;
using MilkTea.Shared.Extensions;

namespace MilkTea.Application.UseCases.Users
{
    public class EmployeeUpdateProfileUseCase(IUserRepository userRepository,
                                                IEmployeeRepository employeeRepository,
                                                IGenderRepository genderRepository,
                                                IUnitOfWork unitOfWork)
    {
        private readonly IUserRepository _vUserRepository = userRepository;
        private readonly IEmployeeRepository _vEmployeeRepository = employeeRepository;
        private readonly IUnitOfWork _vUnitOfWork = unitOfWork;
        private readonly IGenderRepository _vGenderRepository = genderRepository;

        public async Task<EmployeeUpdateProfileResult> Execute(EmployeeUpdateProfileCommand command)
        {
            EmployeeUpdateProfileResult result = new();
            result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);
            if (command.UserID <= 0) return SendMessageError(result, ErrorCode.E0036, "UserID");

            var user = await _vUserRepository.GetByIdAsync(command.UserID);
            if (user == null) return SendMessageError(result, ErrorCode.E0001, "User");

            var employee = await _vEmployeeRepository.GetByIdAsync(user.EmployeesID);
            if (employee == null) return SendMessageError(result, ErrorCode.E0001, "Employee");

            if (!command.Email.IsNullOrWhiteSpace() && command.Email != employee.Email)
            {
                var isEmailExist = await _vEmployeeRepository.IsEmailExistAsync(command.Email!, employee.ID);
                if (isEmailExist)
                    return SendMessageError(result, ErrorCode.E0002, "Email");
            }


            if (!(command.CellPhone.IsNullOrWhiteSpace()) && command.CellPhone != employee.CellPhone)
            {
                var isPhoneExist = await _vEmployeeRepository.IsCellPhoneExistAsync(command.CellPhone!, employee.ID);
                if (isPhoneExist)
                    return SendMessageError(result, ErrorCode.E0002, "CellPhone");
            }

            // Validate
            var isValid = UpdateManyIfValid(result,
                    (command.FullName, v => employee.FullName = v, nameof(command.FullName)),
                    (command.IdentityCode, v => employee.IdentityCode = v, nameof(command.IdentityCode)),
                    (command.Email, v => employee.Email = v, nameof(command.Email)),
                    (command.Address, v => employee.Address = v, nameof(command.Address)),
                    (command.CellPhone, v => employee.CellPhone = v, nameof(command.CellPhone)),
                    (command.BankName, v => employee.BankName = v, nameof(command.BankName)),
                    (command.BankAccountName, v => employee.BankAccountName = v, nameof(command.BankAccountName)),
                    (command.BankAccountNumber, v => employee.BankAccountNumber = v, nameof(command.BankAccountNumber))
                                            );

            if (command.GenderID.HasValue)
            {
                if (command.GenderID <= 0) return SendMessageError(result, ErrorCode.E0036, nameof(command.GenderID));
                var isExist = await _vGenderRepository.ExistsGenderAsync(command.GenderID.Value);
                if (!isExist) return SendMessageError(result, ErrorCode.E0001, "GenderID");
                employee.GenderID = command.GenderID.Value;
            }

            if (command.BirthDay.HasValue)
            {
                var birthDay = command.BirthDay.Value.Date;
                var today = DateTime.UtcNow.Date;
                if (birthDay > today) return SendMessageError(result, ErrorCode.E0036, nameof(command.BirthDay));
                if (birthDay < today.AddYears(-120)) return SendMessageError(result, ErrorCode.E0036, nameof(command.BirthDay));
                if (birthDay > today.AddYears(-18)) return SendMessageError(result, ErrorCode.E0036, nameof(command.BirthDay));
                employee.BirthDay = birthDay.ToString("dd/MM/yyyy");
            }

            //if (command.BankQRCode != null) employee.BankQRCode = command.BankQRCode;

            employee.LastUpdatedBy = command.UserID;
            employee.LastUpdatedDate = DateTime.UtcNow;

            _vEmployeeRepository.Update(employee);
            await _vUnitOfWork.CommitAsync();

            return result;
        }

        private EmployeeUpdateProfileResult SendMessageError(
            EmployeeUpdateProfileResult result,
            string errorCode,
            params string[] values)
        {
            if (values != null && values.Length > 0)
            {
                result.ResultData.Add(errorCode, values.ToList());
            }
            return result;
        }

        private bool UpdateManyIfValid(
            EmployeeUpdateProfileResult result,
            params (string? Value, Action<string> Setter, string FieldName)[] fields)
        {
            foreach (var (value, setter, fieldName) in fields)
            {
                if (value == null) continue;

                if (string.IsNullOrWhiteSpace(value))
                {
                    SendMessageError(result, ErrorCode.E0036, fieldName);
                    return false;
                }

                setter(value.Trim());
            }

            return true;
        }
    }

}
