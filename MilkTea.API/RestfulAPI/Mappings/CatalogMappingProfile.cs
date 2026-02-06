using AutoMapper;
using MilkTea.API.RestfulAPI.DTOs.Catalog.Responses;
using MilkTea.API.RestfulAPI.DTOs.Common;
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
            #region Common
            // Dinner Table Base
            CreateMap<TableDto, DinnerTableBaseDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code ?? string.Empty))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty))
                .ForMember(d => d.Position, o => o.MapFrom(s => s.Position))
                .ForMember(d => d.NumberOfSeats, o => o.MapFrom(s => s.NumberOfSeats ?? 0))
                .ForMember(d => d.Status, o => o.MapFrom(s => s))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note));

            CreateMap<TableDto, StatusBaseDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.StatusId ?? 0))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.StatusName ?? string.Empty));
            #endregion Common

            #region Get Table Empty
            CreateMap<TableDto, GetTableEmptyResponseDto>()
                .IncludeBase<TableDto, DinnerTableBaseDto>()
                .ForMember(d => d.EmptyImg, o => o.MapFrom(s => s.EmptyImg));
            #endregion Get Table Empty

            CreateMap<TableDto, DTOs.Responses.DinnerTableDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code ?? string.Empty))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty))
                .ForMember(d => d.Position, o => o.MapFrom(s => s.Position))
                .ForMember(d => d.NumberOfSeats, o => o.MapFrom(s => s.NumberOfSeats ?? 0))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.StatusName))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note));


        }
    }
}

