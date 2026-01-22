using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Requests;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Commands.Users;
using MilkTea.Application.UseCases.Users;
using MilkTea.Infrastructure.Authentication.JWT.Extensions;
using MilkTea.Shared.Domain.Constants;


namespace MilkTea.API.RestfulAPI.Controllers.Auth
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(LoginWithUserNameUseCase loginUseCase,
                                LogoutUseCase logoutUseCase,
                                RefreshAccessTokenUseCase refreshAccessTokenUseCase,
                                IMapper mapper) : BaseController
    {
        private readonly LoginWithUserNameUseCase _vLoginUseCase = loginUseCase;
        private readonly LogoutUseCase _vLogoutUseCase = logoutUseCase;
        private readonly RefreshAccessTokenUseCase _vRefreshAccessTokenUseCase = refreshAccessTokenUseCase;
        private readonly IMapper _vMapper = mapper;

        [HttpPost("login")]
        public async Task<ResponseDto> Login(LoginRequestDto request)
        {
            var vData = await _vLoginUseCase.Execute(new LoginCommand { Password = request.Password, UserName = request.Username });
            if (vData.ResultData.HasData)
            {
                return SendError(vData.ResultData);
            }
            var vResponse = _vMapper.Map<LoginResponseDto>(vData);
            return SendSuccess(vResponse);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ResponseDto> Logout([FromBody] LogoutRequestDto request)
        {
            var vUserID = User.GetUserId();
            if (!int.TryParse(vUserID, out var vUserIdInt))
            {
                return SendTokenError();
            }

            var vData = await _vLogoutUseCase.Execute(new LogoutCommand
            {
                UserId = vUserIdInt,
                RefreshToken = request.RefreshToken
            });

            if (vData.ResultData.HasData)
            {
                return SendError(vData.ResultData);
            }

            if (vData.ResultData.GetMeta(MetaKey.TOKEN_ERROR) is true)
            {
                return SendTokenError();
            }
            return SendSuccess();
        }

        [HttpPost("refresh-token")]
        public async Task<ResponseDto> RefreshAccessToken(
            [FromBody] RefreshAccessTokenRequestDto request)
        {

            var vData = await _vRefreshAccessTokenUseCase.Execute(new RefreshAccessTokenCommand
            {
                RefreshToken = request.RefreshToken
            });

            if (vData.ResultData.GetMeta(MetaKey.TOKEN_ERROR) is true)
            {
                return SendTokenError();
            }

            if (vData.ResultData.HasData)
            {
                return SendError(vData.ResultData);
            }

            var response = new RefreshAccessTokenResponseDto
            {
                AccessToken = vData.AccessToken,
                ExpiresAt = vData.AccessTokenExpiresAt
            };

            return SendSuccess(response);
        }
    }
}
