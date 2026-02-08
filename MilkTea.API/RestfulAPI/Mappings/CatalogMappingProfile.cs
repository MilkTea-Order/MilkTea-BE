using AutoMapper;
using MilkTea.API.RestfulAPI.DTOs.Catalog.Responses;
using MilkTea.API.RestfulAPI.DTOs.Common;
using MilkTea.Application.Models.Catalog;

namespace MilkTea.API.RestfulAPI.Mappings
{

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
                .ForMember(d => d.Status, o => o.MapFrom(s => new StatusBaseDto { ID = s.StatusId ?? 0, Name = s.StatusName ?? string.Empty }))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note));

            // Menu Group Base
            CreateMap<MenuGroupDto, MenuGroupBaseDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.MenuGroupId))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.MenuGroupName))
                .ForMember(d => d.Status, o => o.MapFrom(s => new StatusBaseDto { ID = s.StatusId, Name = s.StatusName ?? string.Empty }));

            // Menu Base
            CreateMap<MenuItemDto, MenuBaseDto>()
                 .ForMember(d => d.ID, o => o.MapFrom(s => s.MenuId))
                 .ForMember(d => d.Name, o => o.MapFrom(s => s.MenuName))
                 .ForMember(d => d.Code, o => o.MapFrom(s => s.MenuCode))
                 .ForMember(d => d.Status, o => o.MapFrom(s => new StatusBaseDto { ID = s.StatusId, Name = s.StatusName ?? string.Empty }))
                 .ForMember(d => d.MenuGroupID, o => o.MapFrom(s => s.MenuGroupId))
                 .ForMember(d => d.Note, o => o.MapFrom(s => s.Note));

            // Size Base
            CreateMap<MenuSizePriceDto, SizeBaseDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.SizeId))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.SizeName))
                .ForMember(d => d.RankIndex, o => o.MapFrom(s => s.RankIndex));

            // Price Size of Menu
            CreateMap<MenuSizePriceDto, PriceBaseDto>()
                .ForMember(d => d.PriceListID, o => o.MapFrom(s => s.PriceListId))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
                .ForMember(d => d.Currency, o => o.MapFrom(s => new CurrencyBaseDto
                {
                    ID = s.CurrencyId,
                    Name = s.CurrencyName ?? string.Empty,
                    Code = s.CurrencyCode ?? string.Empty
                }));

            #endregion Common

            #region Get Menu Group Available
            CreateMap<MenuGroupDto, GetGroupMenuAvailableResponseDto>()
                .IncludeBase<MenuGroupDto, MenuGroupBaseDto>()
                .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity));
            #endregion Get Menu Group Available

            #region Get Menu Item of Group
            CreateMap<MenuItemDto, GetMenuItemOfGroupResponseDto>()
                .IncludeBase<MenuItemDto, MenuBaseDto>();
            #endregion Get Menu Item of Group

            #region Get Menu Item Available of Group
            CreateMap<MenuItemDto, GetMenuItemAvailableOfGroupResponseDto>()
                .IncludeBase<MenuItemDto, MenuBaseDto>();
            #endregion Get Menu Item Available of Group

            #region Menu size of Menu
            CreateMap<MenuSizePriceDto, GetMenuSizeOfMenuResponseDto>()
                .IncludeBase<MenuSizePriceDto, SizeBaseDto>()
                .ForMember(d => d.Price, o => o.MapFrom(s => s));
            #endregion Menu size of Menu

            #region Get Table Empty
            CreateMap<TableDto, GetTableEmptyResponseDto>()
                .IncludeBase<TableDto, DinnerTableBaseDto>()
                .ForMember(d => d.EmptyImg, o => o.MapFrom(s => s.EmptyImg));
            #endregion Get Table Empty

            #region Get Table By status
            CreateMap<TableDto, DTOs.Responses.DinnerTableDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code ?? string.Empty))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty))
                .ForMember(d => d.Position, o => o.MapFrom(s => s.Position))
                .ForMember(d => d.NumberOfSeats, o => o.MapFrom(s => s.NumberOfSeats ?? 0))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.StatusName))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note));
            #endregion Get Table By status



        }
    }
}

