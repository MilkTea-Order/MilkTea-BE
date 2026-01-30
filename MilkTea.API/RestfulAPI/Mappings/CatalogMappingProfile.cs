using AutoMapper;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Models.Catalog;

namespace MilkTea.API.RestfulAPI.Mappings
{
    /// <summary>
    /// API layer mappings: Application DTOs -> API Response DTOs.
    /// </summary>
    public class CatalogMappingProfile : Profile
    {
        public CatalogMappingProfile()
        {


            CreateMap<TableDto, DTOs.Responses.DinnerTableDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code ?? string.Empty))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty))
                .ForMember(d => d.Position, o => o.MapFrom(s => s.Position))
                .ForMember(d => d.NumberOfSeats, o => o.MapFrom(s => s.NumberOfSeats ?? 0))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.StatusName))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note));




            CreateMap<TableDto, GetTableEmptyResponseDto>()
                .ForMember(d => d.TableID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.TableCode, o => o.MapFrom(s => s.Code))
                .ForMember(d => d.TableName, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.NumberOfSeat, o => o.MapFrom(s => s.NumberOfSeats))
                .ForMember(d => d.TableNote, o => o.MapFrom(s => s.Note))
                .ForMember(d => d.StatusID, o => o.MapFrom(s => s.StatusId))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.StatusName))
                .ForMember(d => d.EmptyImg, o => o.MapFrom(s => s.Img));

        }
    }
}

