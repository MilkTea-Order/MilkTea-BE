using MilkTea.Application.DTOs.Users;
using MilkTea.Application.Ports.Identity;
using MilkTea.Application.Queries.Users;
using MilkTea.Application.Results.Users;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories.Users;

namespace MilkTea.Application.UseCases.Users
{
    public class GetUserProfileUseCase(IUserRepository userRepository, ICurrentUser currentUser)
    {
        private readonly IUserRepository _vUserRepository = userRepository;
        private readonly ICurrentUser _currentUser = currentUser;

        public async Task<GetUserProfileResult> Execute(GetUserProfileQuery query)
        {
            GetUserProfileResult result = new();
            var userId = _currentUser.UserId;
            var user = await _vUserRepository.GetUserWithEmployeeAsync(userId);
            if (user == null)
            {
                result.ResultData.Add(ErrorCode.E0001, nameof(userId));
                return result;
            }

            if (user.Employee == null)
            {
                result.ResultData.Add(ErrorCode.E0001, nameof(userId));
                return result;
            }
            result.User = new UserProfileDto
            {
                UserId = user.ID,
                UserName = user.UserName,
                EmployeeId = user.Employee.ID,
                EmployeeCode = user.Employee.Code,
                FullName = user.Employee.FullName,
                GenderId = user.Employee.GenderID,
                GenderName = user.Employee.Gender?.Name,
                BirthDay = user.Employee.BirthDay,
                IdentityCode = user.Employee.IdentityCode,
                Email = user.Employee.Email,
                Address = user.Employee.Address,
                CellPhone = user.Employee.CellPhone,
                PositionId = user.Employee.PositionID,
                PositionName = user.Employee.Position?.Name,
                StatusId = user.Employee.StatusID,
                StatusName = user.Employee.Status?.Name,
                StartWorkingDate = user.Employee.StartWorkingDate,
                EndWorkingDate = user.Employee.EndWorkingDate,
                BankName = user.Employee.BankName,
                BankAccountName = user.Employee.BankAccountName,
                BankAccountNumber = user.Employee.BankAccountNumber,
                BankQrCodeBase64 = user.Employee.BankQRCode == null
                    ? null
                    : $"data:image/png;base64,{Convert.ToBase64String(user.Employee.BankQRCode)}"
                ,
                CreatedDate = user.Employee.CreatedDate,
                LastUpdatedDate = user.Employee.LastUpdatedDate
            };
            return result;
        }
    }
}
