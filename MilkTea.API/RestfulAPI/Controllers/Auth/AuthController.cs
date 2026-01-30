using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Requests;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Features.Users.Commands;
using MilkTea.Shared.Domain.Constants;


namespace MilkTea.API.RestfulAPI.Controllers.Auth
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(ISender sender,
                                IMapper mapper) : BaseController
    {
        private readonly ISender _vSender = sender;
        private readonly IMapper _vMapper = mapper;

        [HttpPost("login")]
        public async Task<ResponseDto> Login([FromBody] LoginRequestDto request)
        {
            var command = new LoginCommand
            {
                Password = request.Password,
                UserName = request.Username
            };

            var result = await _vSender.Send(command);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }

            var response = _vMapper.Map<LoginResponseDto>(result);
            return SendSuccess(response);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ResponseDto> Logout([FromBody] LogoutRequestDto request)
        {
            var command = new LogoutCommand
            {
                RefreshToken = request.RefreshToken
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

        [HttpPost("refresh-token")]
        public async Task<ResponseDto> RefreshAccessToken([FromBody] RefreshAccessTokenRequestDto request)
        {
            var command = new RefreshAccessTokenCommand
            {
                RefreshToken = request.RefreshToken
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

            var response = _vMapper.Map<RefreshAccessTokenResponseDto>(result);

            return SendSuccess(response);
        }
    }
}
