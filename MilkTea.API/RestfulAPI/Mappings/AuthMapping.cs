using AutoMapper;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Results.Users;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.API.RestfulAPI.Mappings
{
    public class AuthMapping : Profile
    {
        public AuthMapping()
        {
            CreateMap<LoginWithUserNameResult, LoginResponseDto>()
                .ForMember(
                    d => d.ExpiresAt,
                    o => o.MapFrom(s => s.AccessTokenExpiresAt)
                )
                .ForMember(
                    d => d.User,
                    o => o.MapFrom(s => new UserResponseDto
                    {
                        Id = s.UserId,
                        DateLogin = (DateTime)s.ResultData.GetMeta(MetaKey.DATE_LOGIN)!
                    })
                );
        }
    }
}
