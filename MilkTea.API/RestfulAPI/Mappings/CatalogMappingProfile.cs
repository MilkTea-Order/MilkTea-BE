using AutoMapper;
using MilkTea.API.RestfulAPI.DTOs.Catalog.Responses;
using MilkTea.API.RestfulAPI.DTOs.Common;
using MilkTea.Application.Features.Catalog.Models.Dtos;
using MilkTea.Application.Features.Catalog.Models.Dtos.Currency;
using MilkTea.Application.Features.Catalog.Models.Dtos.Menu;
using MilkTea.Application.Features.Catalog.Models.Dtos.Table;
using MenuGroupDto = MilkTea.Application.Features.Catalog.Models.Dtos.Menu.MenuGroupDto;


namespace MilkTea.API.RestfulAPI.Mappings
{

    public class CatalogMappingProfile : Profile
    {
        public CatalogMappingProfile()
        {
            #region Common
            // Status Base
            CreateMap<StatusDto, StatusBaseDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name));
            
            // Dinner Table Base
            CreateMap<TableDto, DinnerTableBaseDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code ?? string.Empty))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty))
                .ForMember(d => d.Position, o => o.MapFrom(s => s.Position))
                .ForMember(d => d.NumberOfSeats, o => o.MapFrom(s => s.NumberOfSeats ?? 0))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note));

            
            // Menu Group Base
            CreateMap<MenuGroupDto, MenuGroupBaseDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.MenuGroupId))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.MenuGroupName))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status));

            CreateMap<MenuDto, MenuBaseDto>()
                 .ForMember(d => d.ID, o => o.MapFrom(s => s.MenuId))
                 .ForMember(d => d.Name, o => o.MapFrom(s => s.MenuName))
                 .ForMember(d => d.Code, o => o.MapFrom(s => s.MenuCode))
                 .ForMember(d => d.Image, o => o.MapFrom(s => s.MenuImage))
                 .ForMember(d => d.Status, o => o.MapFrom(s => s.Status))
                 .ForMember(d => d.Unit, o => o.MapFrom(s => new UnitBaseDto { ID = s.UnitId, Name = s.UnitName ?? string.Empty }))
                 .ForMember(d => d.MenuGroupID, o => o.MapFrom(s => s.MenuGroupId))
                 .ForMember(d => d.Note, o => o.MapFrom(s => s.Note));

            // Size Base
            CreateMap<SizeAndPriceDto, SizeBaseDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Size.SizeId))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Size.SizeName))
                .ForMember(d => d.RankIndex, o => o.MapFrom(s => s.Size.RankIndex));

            // Price Size of Menu
            CreateMap<CurrencyDto, CurrencyBaseDto>()
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id));        
            CreateMap<SizeAndPriceDto, PriceBaseDto>()
                .ForMember(d => d.PriceListID, o => o.MapFrom(s => s.Price.PriceListId))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Price.Price))
                .ForMember(d => d.Currency, o => o.MapFrom(s => s.Price.Currency));

            #endregion Common

            #region Get Menu Group Available
            CreateMap<MenuGroupDto, GetGroupMenuAvailableResponseDto>()
                .IncludeBase<MenuGroupDto, MenuGroupBaseDto>()
                .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity));
            #endregion Get Menu Group Available

            #region Get Menu Item of Group
            CreateMap<MenuDto, GetMenuItemOfGroupResponseDto>()
                .IncludeBase<MenuDto, MenuBaseDto>();
            #endregion Get Menu Item of Group

            #region Get Menu Item Available of Group
            CreateMap<MenuDto, GetMenuItemAvailableOfGroupResponseDto>()
                .IncludeBase<MenuDto, MenuBaseDto>();
            #endregion Get Menu Item Available of Group

            #region Get Menu Item Available of Group And Name
            CreateMap<MenuDto, GetMenuItemAvailableOfGroupAndNameResponseDto>()
                .IncludeBase<MenuDto, MenuBaseDto>();
            #endregion Get Menu Item Available of Group And Name

            #region Menu size of Menu
            CreateMap<SizeAndPriceDto, GetMenuSizeOfMenuResponseDto>()
                .IncludeBase<SizeAndPriceDto, SizeBaseDto>()
                .ForMember(d => d.Price, o => o.MapFrom(s => s));
            #endregion Menu size of Menu

            #region Get Table 
            CreateMap<Application.Features.Catalog.Models.Dtos.Table.TableDto, GetTableResponseDto>()
                .IncludeBase<Application.Features.Catalog.Models.Dtos.Table.TableDto, DinnerTableBaseDto>()
                .ForMember(d => d.EmptyImg, o => o.MapFrom(s => s.EmptyImg))
                .ForMember(d => d.UsingImg, o => o.MapFrom(s => s.UsingImg));
            #endregion Get Table 

            #region Get Table By status
            //CreateMap<TableDto, DTOs.Responses.DinnerTableDto>()
            //    .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
            //    .ForMember(d => d.Code, o => o.MapFrom(s => s.Code ?? string.Empty))
            //    .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty))
            //    .ForMember(d => d.Position, o => o.MapFrom(s => s.Position))
            //    .ForMember(d => d.NumberOfSeats, o => o.MapFrom(s => s.NumberOfSeats ?? 0))
            //    .ForMember(d => d.StatusName, o => o.MapFrom(s => s.StatusName))
            //    .ForMember(d => d.Note, o => o.MapFrom(s => s.Note));
            #endregion Get Table By status
        }
    }
}

