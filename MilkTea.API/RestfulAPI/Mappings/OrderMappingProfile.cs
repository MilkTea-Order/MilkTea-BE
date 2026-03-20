using AutoMapper;
using MilkTea.API.RestfulAPI.DTOs.Common;
using MilkTea.API.RestfulAPI.DTOs.Order.Responses;
using MilkTea.API.RestfulAPI.DTOs.Orders.Common;
using MilkTea.API.RestfulAPI.DTOs.Orders.Responses;
using MilkTea.Application.Features.Orders.Models.Dtos;
using MilkTea.Application.Models.Orders;

namespace MilkTea.API.RestfulAPI.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            #region CommonBase
            // Base DinnerTable
            CreateMap<Table, DinnerTableBaseDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code ?? string.Empty))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty))
                .ForMember(d => d.Position, o => o.MapFrom(s => s.Position))
                .ForMember(d => d.NumberOfSeats, o => o.MapFrom(s => s.NumberOfSeats ?? 0))
                .ForMember(d => d.Status, o => o.MapFrom(s => new StatusBaseDto
                {
                    ID = s.StatusId ?? 0,
                    Name = s.StatusName ?? string.Empty
                }))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note));

            CreateMap<TableDto, DinnerTableBaseDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code ?? string.Empty))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty))
                .ForMember(d => d.Position, o => o.MapFrom(s => s.Position))
                .ForMember(d => d.NumberOfSeats, o => o.MapFrom(s => s.NumberOfSeats ?? 0))
                .ForMember(d => d.Status, o => o.MapFrom(s => new StatusBaseDto
                {
                    ID = s.StatusId ?? 0,
                    Name = s.StatusName ?? string.Empty
                }))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note));


            // Base Order Status
            CreateMap<OrderStatus, StatusBaseDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty));

            CreateMap<OrderStatusDto, StatusBaseDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty));


            // Base Menu
            CreateMap<Menu, MenuBaseDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code ?? string.Empty))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty))
                .ForMember(d => d.MenuGroupID, o => o.MapFrom(s => s.MenuGroupId))
                .ForMember(d => d.Status, o => o.MapFrom(s => new StatusBaseDto { ID = s.StatusId ?? 0, Name = s.StatusName ?? string.Empty }))
                .ForMember(d => d.Unit, o => o.MapFrom(s => new UnitBaseDto { ID = s.UnitId ?? 0, Name = s.UnitName ?? string.Empty }))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note));

            CreateMap<MenuDto, MenuBaseDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code ?? string.Empty))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty))
                .ForMember(d => d.MenuGroupID, o => o.MapFrom(s => s.MenuGroupId))
                .ForMember(d => d.Status, o => o.MapFrom(s => new StatusBaseDto { ID = s.StatusId ?? 0, Name = s.StatusName ?? string.Empty }))
                .ForMember(d => d.Unit, o => o.MapFrom(s => new UnitBaseDto { ID = s.UnitId ?? 0, Name = s.UnitName ?? string.Empty }))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note));

            // Base Size
            CreateMap<Size, SizeBaseDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty))
                .ForMember(d => d.RankIndex, o => o.MapFrom(s => s.RankIndex ?? 0));

            CreateMap<SizeDto, SizeBaseDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty))
                .ForMember(d => d.RankIndex, o => o.MapFrom(s => s.RankIndex ?? 0));
            #endregion

            #region Common order
            CreateMap<MilkTea.Application.Features.Orders.Models.Dtos.OrderDto, MilkTea.API.RestfulAPI.DTOs.Orders.Common.OrderDto>()
            .ForMember(dest => dest.OrderID, opt => opt.MapFrom(src => src.OrderId))
            //Order
            .ForMember(dest => dest.OrderBy, opt => opt.MapFrom(src => src.OrderBy))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
            //Create
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            //Note
            .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
            // Payment
            .ForMember(dest => dest.PaymentBy, opt => opt.MapFrom(src => src.PaymentBy))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
            .ForMember(dest => dest.PaymentAmount, opt => opt.MapFrom(src => src.PaymentAmount))
            //Total Amount
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            // Common infor (Table  + Status)
            .ForMember(d => d.DinnerTable, o => o.MapFrom(s => s.DinnerTable))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status));


            CreateMap<OrderLine, OrderDetailDto>()
              .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
              .ForMember(d => d.OrderID, o => o.MapFrom(s => s.OrderId))
              .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity))
              .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
              .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.CreatedBy ?? 0))
              .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate ?? default))
              .ForMember(d => d.CancelledBy, o => o.MapFrom(s => s.CancelledBy))
              .ForMember(d => d.CancelledDate, o => o.MapFrom(s => s.CancelledDate))
              .ForMember(d => d.Note, o => o.MapFrom(s => s.Note))
              .ForMember(d => d.KindOfHotpot1ID, o => o.MapFrom(s => s.KindOfHotpot1Id))
              .ForMember(d => d.Menu, o => o.MapFrom(s => s.Menu))
              .ForMember(d => d.Size, o => o.MapFrom(s => s.Size));

            CreateMap<Table, DinnerTableDto>()
              .IncludeBase<Table, DinnerTableBaseDto>()
              .ForMember(d => d.UsingImg, o => o.MapFrom(s => s.UsingImg))
              .ForMember(d => d.EmptyImg, o => o.MapFrom(s => s.EmptyImg));

            CreateMap<TableDto, DinnerTableDto>()
              .IncludeBase<TableDto, DinnerTableBaseDto>()
              .ForMember(d => d.UsingImg, o => o.MapFrom(s => s.UsingImg))
              .ForMember(d => d.EmptyImg, o => o.MapFrom(s => s.EmptyImg));
            #endregion Common order

            #region GetOrdersByOrderByAndStatusResponseDto

            CreateMap<Application.Features.Orders.Models.Dtos.OrderDto, GetOrdersByOrderByAndStatusResponseDto>()
                .ForMember(d => d.OrderID, o => o.MapFrom(s => s.OrderId))
                .ForMember(d => d.OrderDate, o => o.MapFrom(s => s.OrderDate ?? default))
                .ForMember(d => d.OrderBy, o => o.MapFrom(s => s.OrderBy ?? 0))
                .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate ?? default))
                .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.CreatedBy ?? 0))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note))
                .ForMember(d => d.TotalAmount, o => o.MapFrom(s => s.TotalAmount))
                .ForMember(d => d.DinnerTable, o => o.MapFrom(s => s.DinnerTable))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status));

            #endregion GetOrdersByOrderByAndStatusResponseDto

            #region GetOrderDetailByIDAndStatusResponseDto
            CreateMap<OrderDetail, GetOrderDetailByIDAndStatusResponseDto>()
                .ForMember(d => d.OrderID, o => o.MapFrom(s => s.OrderId))
                .ForMember(d => d.OrderDate, o => o.MapFrom(s => s.OrderDate ?? default))
                .ForMember(d => d.OrderBy, o => o.MapFrom(s => s.OrderBy ?? 0))
                .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate ?? default))
                .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.CreatedBy ?? 0))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note))
                .ForMember(d => d.TotalAmount, o => o.MapFrom(s => s.TotalAmount))
                .ForMember(d => d.DinnerTable, o => o.MapFrom(s => s.DinnerTable))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status))
                .ForMember(d => d.OrderDetails, o => o.MapFrom(s => s.OrderDetails));
            #endregion GetOrderDetailByIDAndStatusResponseDto

            #region Create Order
            CreateMap<Order, CreateOrderResponseDto>()
                .ForMember(d => d.OrderID, o => o.MapFrom(s => s.OrderId))
                .ForMember(d => d.OrderDate, o => o.MapFrom(s => s.OrderDate ?? default))
                .ForMember(d => d.OrderBy, o => o.MapFrom(s => s.OrderBy ?? 0))
                .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate ?? default))
                .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.CreatedBy ?? 0))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note))
                .ForMember(d => d.TotalAmount, o => o.MapFrom(s => s.TotalAmount))
                .ForMember(d => d.DinnerTable, o => o.MapFrom(s => s.DinnerTable))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status));
            #endregion Create Order

            #region Order Report
            CreateMap<StaticDto, StaticOrderReportResponseDto>()
            .ForMember(d => d.TotalAmountCash, o => o.MapFrom(s => s.TotalAmountCash))
            .ForMember(d => d.TotalAmountShopee, o => o.MapFrom(s => s.TotalAmountShopee))
            .ForMember(d => d.TotalAmountBank, o => o.MapFrom(s => s.TotalAmountBank))
            .ForMember(d => d.TotalAmountGrab, o => o.MapFrom(s => s.TotalAmountGrab))
            .ForMember(d => d.TotalAmount, o => o.MapFrom(s => s.TotalAmount));

            CreateMap<ReportOrderDto, GetOrderReportResponseDto>()
            .ForMember(d => d.Orders, o => o.MapFrom(s => s.Orders))
            .ForMember(d => d.Statics, o => o.MapFrom(s => s.Statics));
            #endregion
        }
    }
}

