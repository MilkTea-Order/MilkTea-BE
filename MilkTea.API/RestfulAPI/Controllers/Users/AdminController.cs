using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Requests;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Features.Users.Commands;

namespace MilkTea.API.RestfulAPI.Controllers.Users
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController(ISender sender) : BaseController
    {
        private readonly ISender _sender = sender;

        [Authorize(Roles = "Admin")]
        [HttpPut("/update-user/{userId}")]
        public async Task<ResponseDto> AdminUpdateUser(int userId, [FromBody] AdminUpdateUserRequestDto request)
        {
            var command = new AdminUpdateUserCommand
            {
                UserID = userId,
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
            };

            var result = await _sender.Send(command);

            if (result.ResultData.HasData) return SendError(result.ResultData);
            return SendSuccess();
        }
    }
}
