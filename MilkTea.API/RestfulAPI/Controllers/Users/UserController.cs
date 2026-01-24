using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Requests;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Commands.Users;
using MilkTea.Application.Queries.Users;
using MilkTea.Application.UseCases.Users;

namespace MilkTea.API.RestfulAPI.Controllers.Users
{
    [ApiController]
    [Route("api/user")]
    public class UserController(
                    UpdatePasswordUseCase updatePasswordUseCase,
                    EmployeeUpdateProfileUseCase employeeUpdateProfileUseCase,
                    GetUserProfileUseCase getUserProfileUseCase,
                    IMapper mapper) : BaseController
    {
        private readonly UpdatePasswordUseCase _vUpdatePasswordUseCase = updatePasswordUseCase;
        private readonly EmployeeUpdateProfileUseCase _vEmployeeUpdateProfileUseCase = employeeUpdateProfileUseCase;
        private readonly GetUserProfileUseCase _vGetUserProfileUseCase = getUserProfileUseCase;
        private readonly IMapper _vMapper = mapper;

        [Authorize]
        [HttpPatch("update-password")]
        public async Task<ResponseDto> UpdatePassword(UpdatePasswordRequestDto request)
        {
            var vData = await _vUpdatePasswordUseCase.Execute(new UpdatePasswordCommand
            {
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
        [HttpPatch("me/update-profile")]
        public async Task<ResponseDto> UpdateProfile([FromForm] EmployeeUpdateProfileRequestDto request)
        {
            if (!ModelState.IsValid)
            {

                return SendError(ModelState);
            }
            var result = await _vEmployeeUpdateProfileUseCase.Execute(new EmployeeUpdateProfileCommand
            {
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

        [Authorize]
        [HttpGet("me")]
        public async Task<ResponseDto> GetMe()
        {
            var result = await _vGetUserProfileUseCase.Execute(new GetUserProfileQuery());

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            var responseDto = _vMapper.Map<GetUserProfileResponseDto>(result.User);

            return SendSuccess(responseDto);
        }
    }
}
