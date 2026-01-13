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
    [Route("api/admin")]
    public class AdminController(AdminUpdateUserUseCase adminUpdateUserUseCase) : BaseController
    {
        private readonly AdminUpdateUserUseCase _vAdminUpdateUserUseCase = adminUpdateUserUseCase;
        [Authorize(Roles = "Admin")]
        [HttpPut("/update-user/{userId}")]
        public async Task<ResponseDto> AdminUpdateUser(int userId, [FromBody] AdminUpdateUserRequestDto request)
        {
            //if (!ModelState.IsValid) return SendError(ModelState);

            var adminIdStr = User.GetUserId();
            if (!int.TryParse(adminIdStr, out var adminId)) return SendError();

            var result = await _vAdminUpdateUserUseCase.Execute(
                new AdminUpdateUserCommand
                {
                    UserID = userId,
                    AdminID = adminId,

                    FullName = request.FullName,
                    GenderID = request.GenderID,
                    BirthDay = request.BirthDay,
                    IdentityCode = request.IdentityCode,
                    Email = request.Email,
                    Address = request.Address,
                    CellPhone = request.CellPhone,

                    PositionID = request.PositionID,
                    StartWorkingDate = request.StartWorkingDate,
                    EndWorkingDate = request.EndWorkingDate,
                    StatusID = request.StatusID,

                    SalaryByHour = request.SalaryByHour,
                    CalcSalaryByMinutes = request.CalcSalaryByMinutes,

                    ShiftFrom = request.ShiftFrom,
                    ShiftTo = request.ShiftTo,
                    IsBreakTime = request.IsBreakTime,
                    BreakTimeFrom = request.BreakTimeFrom,
                    BreakTimeTo = request.BreakTimeTo,

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
