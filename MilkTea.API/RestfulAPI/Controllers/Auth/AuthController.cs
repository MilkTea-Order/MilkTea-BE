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

        [HttpPost("otp")]
        public async Task<ResponseDto> CreateOtp(CreateOtpRequestDto request)
        {
            var command = new CreateOtpCommand
            {
                Email = request.Email,
                Function = request.Function
            };
            var result = await _vSender.Send(command);
            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            return SendSuccess(new
            {
                SessionId = result.SessionId,
                ExpiresAt = result.ExpiresAt
            });
        }

        [HttpPost("otp/{sessionId:int}/verify")]
        public async Task<ResponseDto> VerifyOtp(int sessionId, [FromBody] VerifyOtpRequestDto request)
        {
            var command = new VerifyOtpCommand
            {
                SessionId = sessionId,
                OtpCode = request.OtpCode
            };
            var result = await _vSender.Send(command);
            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            if (!string.IsNullOrEmpty(result.ResetToken) && result.ResetTokenExpiresAt.HasValue)
            {
                var response = new VerifyOtpResponseDto
                {
                    ResetPasswordToken = result.ResetToken,
                    ExpiresAt = result.ResetTokenExpiresAt.Value
                };
                return SendSuccess(response);
            }

            return SendSuccess();
        }

        [HttpPost("otp/{sessionId:int}/resend")]
        public async Task<ResponseDto> ResendOtp(
            int sessionId,
            [FromHeader(Name = "Idempotency-Key")] string idempotencyKey,
            [FromBody] ResendOtpRequestDto request)
        {
            var command = new ResendOtpCommand
            {
                SessionId = sessionId,
                Channel = request.Channel,
                IdempotencyKey = idempotencyKey
            };
            var result = await _vSender.Send(command);
            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            return SendSuccess(new
            {
                SessionId = result.SessionId,
                ExpiresAt = result.ExpiresAt
            });
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
