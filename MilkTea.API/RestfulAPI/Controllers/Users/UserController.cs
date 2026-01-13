using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Requests;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Commands.Users;
using MilkTea.Application.UseCases.Users;
using MilkTea.Infrastructure.Authentication.JWT.Extensions;

namespace MilkTea.API.RestfulAPI.Controllers.Users
{
    [ApiController]
    [Route("api/user")]
    public class UserController(
                    UpdatePasswordUseCase updatePasswordUseCase,
                    EmployeeUpdateProfileUseCase employeeUpdateProfileUseCase) : BaseController
    {
        private readonly UpdatePasswordUseCase _vUpdatePasswordUseCase = updatePasswordUseCase;
        private readonly EmployeeUpdateProfileUseCase _vEmployeeUpdateProfileUseCase = employeeUpdateProfileUseCase;

        [Authorize]
        [HttpPost("update-password")]
        public async Task<ResponseDto> UpdatePassword(UpdatePasswordRequestDto request)
        {
            var vUserID = User.GetUserId();
            if (!int.TryParse(vUserID, out var vUserIdInt))
            {
                return SendError();
            }

            var vData = await _vUpdatePasswordUseCase.Execute(new UpdatePasswordCommand
            {
                UserId = vUserIdInt,
                Password = request.Password,
                NewPassword = request.NewPassword
            });

            if (vData.ResultData.HasData)
            {
                return SendError(vData.ResultData);
            }

            return SendSuccess();
        }

        [Authorize]
        [HttpPut("me/profile")]
        public async Task<ResponseDto> UpdateProfile(EmployeeUpdateProfileRequestDto request)
        {

            if (!ModelState.IsValid)
            {
                return SendError(ModelState);
                // Trả về ngay lập tức, KHÔNG GỌI UseCase
            }
            var userId = User.GetUserId();
            if (!int.TryParse(userId, out var vUserId)) return SendError();

            var result = await _vEmployeeUpdateProfileUseCase.Execute(new EmployeeUpdateProfileCommand
            {
                UserID = vUserId,
                FullName = request.FullName,
                GenderID = request.GenderID,
                BirthDay = request.BirthDay,
                IdentityCode = request.IdentityCode,
                Email = request.Email,
                Address = request.Address,
                CellPhone = request.CellPhone,
                BankName = request.BankName,
                BankAccountName = request.BankAccountName,
                BankAccountNumber = request.BankAccountNumber,
                BankQRCode = request.BankQRCode
            });

            if (result.ResultData.HasData) return SendError(result.ResultData);

            return SendSuccess();
        }
    }
}
