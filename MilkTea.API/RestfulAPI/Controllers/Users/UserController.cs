using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Requests;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Features.Users.Commands;
using MilkTea.Application.Features.Users.Queries;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.API.RestfulAPI.Controllers.Users
{
    [ApiController]
    [Route("api/user")]
    public class UserController(
                    ISender sender,
                    IMapper mapper) : BaseController
    {
        private readonly ISender _vSender = sender;
        private readonly IMapper _vMapper = mapper;

        [Authorize]
        [HttpPatch("update-password")]
        public async Task<ResponseDto> UpdatePassword(UpdatePasswordRequestDto request)
        {
            var command = new UpdatePasswordCommand
            {
                Password = request.Password,
                NewPassword = request.NewPassword,
                ConfirmPassword = request.ConfirmPassword
            };

            var result = await _vSender.Send(command);

            if (result.ResultData.GetMeta(MetaKey.TOKEN_ERROR) is true)
            {
                return SendTokenError();
            }

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }

            return SendSuccess();
        }

        [Authorize]
        [HttpPatch("me/update-profile")]
        public async Task<ResponseDto> UpdateProfile([FromForm] EmployeeUpdateProfileRequestDto request)
        {


            var command = new EmployeeUpdateProfileCommand
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
            };

            var result = await _vSender.Send(command);

            if (result.ResultData.GetMeta(MetaKey.TOKEN_ERROR) is true)
            {
                return SendTokenError();
            }

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            return SendSuccess();
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ResponseDto> GetMe()
        {
            var query = new GetUserProfileQuery();
            var result = await _vSender.Send(query);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            var responseDto = _vMapper.Map<GetUserProfileResponseDto>(result.User);

            return SendSuccess(responseDto);
        }
    }
}
