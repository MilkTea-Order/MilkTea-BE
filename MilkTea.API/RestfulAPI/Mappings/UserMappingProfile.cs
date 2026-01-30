using AutoMapper;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Models.Users;

namespace MilkTea.API.RestfulAPI.Mappings
{
    /// <summary>
    /// API layer mappings: Application DTOs -> API Response DTOs.
    /// </summary>
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserProfile, GetUserProfileResponseDto>()
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName ?? string.Empty))
                .ForMember(d => d.EmployeeId, o => o.MapFrom(s => s.EmployeeId))
                .ForMember(d => d.EmployeeCode, o => o.MapFrom(s => s.EmployeeCode))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName ?? string.Empty))
                .ForMember(d => d.GenderID, o => o.MapFrom(s => s.GenderId))
                .ForMember(d => d.GenderName, o => o.MapFrom(s => s.GenderName))
                .ForMember(d => d.BirthDay, o => o.MapFrom(s => s.BirthDay))
                .ForMember(d => d.IdentityCode, o => o.MapFrom(s => s.IdentityCode))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                .ForMember(d => d.CellPhone, o => o.MapFrom(s => s.CellPhone))
                .ForMember(d => d.PositionID, o => o.MapFrom(s => s.PositionId))
                .ForMember(d => d.PositionName, o => o.MapFrom(s => s.PositionName))
                .ForMember(d => d.StatusID, o => o.MapFrom(s => s.StatusId))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.StatusName))
                .ForMember(d => d.StartWorkingDate, o => o.MapFrom(s => s.StartWorkingDate))
                .ForMember(d => d.EndWorkingDate, o => o.MapFrom(s => s.EndWorkingDate))
                .ForMember(d => d.BankName, o => o.MapFrom(s => s.BankName))
                .ForMember(d => d.BankAccountName, o => o.MapFrom(s => s.BankAccountName))
                .ForMember(d => d.BankAccountNumber, o => o.MapFrom(s => s.BankAccountNumber))
                .ForMember(d => d.BankQRCode, o => o.MapFrom(s => s.BankQrCodeBase64))
                .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate))
                .ForMember(d => d.LastUpdatedDate, o => o.MapFrom(s => s.LastUpdatedDate));
        }
    }
}

