using AutoMapper;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Domain.Entities.Users;

namespace MilkTea.API.RestfulAPI.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // Map User entity to GetUserProfileResponseDto
            CreateMap<User, GetUserProfileResponseDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.ID : 0))
                .ForMember(dest => dest.EmployeeCode, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.Code : null))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null))
                .ForMember(dest => dest.GenderID, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.GenderID : 0))
                .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => src.Employee != null && src.Employee.Gender != null ? src.Employee.Gender.Name : null))
                .ForMember(dest => dest.BirthDay, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.BirthDay : null))
                .ForMember(dest => dest.IdentityCode, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.IdentityCode : null))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.Email : null))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.Address : null))
                .ForMember(dest => dest.CellPhone, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.CellPhone : null))
                .ForMember(dest => dest.PositionID, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.PositionID : 0))
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Employee != null && src.Employee.Position != null ? src.Employee.Position.Name : null))
                .ForMember(dest => dest.StatusID, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.StatusID : 0))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Employee != null && src.Employee.Status != null ? src.Employee.Status.Name : null))
                .ForMember(dest => dest.StartWorkingDate, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.StartWorkingDate : null))
                .ForMember(dest => dest.EndWorkingDate, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.EndWorkingDate : null))
                .ForMember(dest => dest.BankName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.BankName : null))
                .ForMember(dest => dest.BankAccountName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.BankAccountName : null))
                .ForMember(dest => dest.BankAccountNumber, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.BankAccountNumber : null))
                .ForMember(dest => dest.BankQRCode, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.BankQRCode : null));
        }
    }
}
