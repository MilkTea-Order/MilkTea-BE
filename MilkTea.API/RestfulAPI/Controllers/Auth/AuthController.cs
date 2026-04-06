using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Auth.Requests;
using MilkTea.API.RestfulAPI.DTOs.Auth.Responses;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Features.Auth.Commands;
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

        [HttpPost("forgot-password")]
        public async Task<ResponseDto> ForgotPassword(ForgotPasswordRequestDto request)
        {
            var command = new ForgotPasswordCommand
            {
                Email = request.Email
            };
            var result = await _vSender.Send(command);
            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            return SendSuccess(new
            {
                ExpiresAt = result.ExpiresAt
            });
        }

        [HttpPost("forgot-password/verify")]
        public async Task<ResponseDto> VerifyOtp(VerifyOtpRequestDto request)
        {
            var command = new VerifyOtpCommand
            {
                Email = request.Email,
                Otp = request.Otp
            };
            var result = await _vSender.Send(command);
            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }

            if (!string.IsNullOrEmpty(result.Token) && result.Expiration.HasValue)
            {
                var response = new VerifyOtpResponseDto
                {
                    ResetPasswordToken = result.Token,
                    ExpiresAt = result.Expiration.Value
                };
                return SendSuccess(response);
            }

            return SendSuccess();
        }

        [HttpPost("forgot-password/resend")]
        public async Task<ResponseDto> ResendOtp(ResendOtpRequestDto request)
        {
            var command = new ResendOtpCommand
            {
                Email = request.Email
            };
            var result = await _vSender.Send(command);
            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            return SendSuccess();
        }

        [HttpPost("forgot-password/reset")]
        public async Task<ResponseDto> ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            var command = new ResetPasswordCommand
            {
                ResetPasswordToken = request.ResetPasswordToken,
                NewPassword = request.NewPassword,
                ConfirmPassword = request.ConfirmPassword
            };

            var result = await _vSender.Send(command);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }

            return SendSuccess();
        }

    }
}
