using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Requests;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Commands.Users;
using MilkTea.Application.UseCases.Users;


namespace MilkTea.API.RestfulAPI.Controllers.Users
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(LoginWithUserNameUseCase loginUseCase, IMapper mapper) : BaseController
    {
        private readonly LoginWithUserNameUseCase _vLoginUseCase = loginUseCase;
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
    }
}

//public async Task<IActionResult> Login(LoginRequestDto request)
//{
//    var vData = await _vLoginUseCase.Execute(new LoginCommand { Password = request.Password, UserName = request.Username });
//    if (vData.ResultData.HasData)
//    {
//        return BadRequest(new ResponseDto<Dictionary<string, object>>
//        {
//            Code = (int)HttpStatusCode.BadRequest,
//            Data = vData.ResultData.GetData(),
//            Message = "Đăng nhập không thành công"
//        });
//    }
//    if (!vData.IsActive)
//    {
//        return Unauthorized(new ResponseDto<string>
//        {
//            Code = (int)HttpStatusCode.Unauthorized,
//            Message = "Tài khoản của bạn chưa được kích hoạt",
//        });
//    }
//    var response = _vMapper.Map<LoginResponseDto>(vData);

//    return Ok(new ResponseDto<LoginResponseDto>
//    {
//        Code = (int)HttpStatusCode.OK,
//        Message = "Đăng nhập thành công",
//        Data = response
//    });
//}