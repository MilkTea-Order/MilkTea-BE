using AutoMapper;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Models.Catalog;
using MilkTea.Application.Models.Orders;

namespace MilkTea.API.RestfulAPI.Mappings
{
    /// <summary>
    /// API layer mappings: Application DTOs -> API Response DTOs.
    /// </summary>
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            // Create order
            CreateMap<CreateOrderResult, CreateOrderResponseDto>()
                .ForMember(d => d.OrderID, o => o.MapFrom(s => s.OrderID ?? 0))
                .ForMember(d => d.BillNo, o => o.MapFrom(s => s.BillNo ?? string.Empty))
                .ForMember(d => d.TotalAmount, o => o.MapFrom(s => s.TotalAmount ?? 0m))
                .ForMember(d => d.OrderDate, o => o.MapFrom(s => s.OrderDate ?? default))
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Items));

            CreateMap<OrderItemResult, OrderItemResponse>()
                .ForMember(d => d.MenuID, o => o.MapFrom(s => s.MenuID))
                .ForMember(d => d.MenuName, o => o.MapFrom(s => s.MenuName ?? string.Empty))
                .ForMember(d => d.SizeID, o => o.MapFrom(s => s.SizeID))
                .ForMember(d => d.SizeName, o => o.MapFrom(s => s.SizeName))
                .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
                .ForMember(d => d.TotalPrice, o => o.MapFrom(s => s.TotalPrice));

            // Cancel order
            CreateMap<CancelOrderResult, CancelOrderResponseDto>()
                .ForMember(d => d.OrderID, o => o.MapFrom(s => s.OrderID ?? 0))
                .ForMember(d => d.BillNo, o => o.MapFrom(s => s.BillNo ?? string.Empty))
                .ForMember(d => d.CancelledDate, o => o.MapFrom(s => s.CancelledDate ?? default));

            // Cancel order details
            CreateMap<CancelOrderDetailsResult, CancelOrderDetailsResponseDto>()
                .ForMember(d => d.OrderID, o => o.MapFrom(s => s.OrderID ?? 0))
                .ForMember(d => d.CancelledDetailIDs, o => o.MapFrom(s => s.CancelledDetailIDs))
                .ForMember(d => d.CancelledDate, o => o.MapFrom(s => s.CancelledDate ?? default));

            // List orders
            CreateMap<Order, GetOrdersByOrderByAndStatusResponseDto>()
                .ForMember(d => d.OrderID, o => o.MapFrom(s => s.OrderId))
                .ForMember(d => d.DinnerTableID, o => o.MapFrom(s => s.DinnerTableId))
                .ForMember(d => d.OrderDate, o => o.MapFrom(s => s.OrderDate ?? default))
                .ForMember(d => d.OrderBy, o => o.MapFrom(s => s.OrderBy ?? 0))
                .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate ?? default))
                .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.CreatedBy ?? 0))
                .ForMember(d => d.StatusID, o => o.MapFrom(s => s.StatusId ?? 0))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note))
                .ForMember(d => d.TotalAmount, o => o.MapFrom(s => s.TotalAmount))
                .ForMember(d => d.DinnerTable, o => o.Ignore())
                .ForMember(d => d.Status, o => o.Ignore())
                .ForMember(d => d.OrderDetails, o => o.Ignore());

            // Order detail
            CreateMap<Application.Models.Orders.OrderDetail, GetOrderDetailByIDAndStatusResponseDto>()
                .ForMember(d => d.OrderID, o => o.MapFrom(s => s.OrderId))
                .ForMember(d => d.DinnerTableID, o => o.MapFrom(s => s.DinnerTableId))
                .ForMember(d => d.OrderDate, o => o.MapFrom(s => s.OrderDate ?? default))
                .ForMember(d => d.OrderBy, o => o.MapFrom(s => s.OrderBy ?? 0))
                .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate ?? default))
                .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.CreatedBy ?? 0))
                .ForMember(d => d.StatusID, o => o.MapFrom(s => s.StatusId ?? 0))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note))
                .ForMember(d => d.TotalAmount, o => o.MapFrom(s => s.TotalAmount))
                .ForMember(d => d.DinnerTable, o => o.MapFrom(s => s.DinnerTable))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status))
                .ForMember(d => d.OrderDetails, o => o.MapFrom(s => s.OrderDetails));

            CreateMap<TableDto, DTOs.Responses.DinnerTableDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code ?? string.Empty))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty))
                .ForMember(d => d.Position, o => o.MapFrom(s => s.Position))
                .ForMember(d => d.NumberOfSeats, o => o.MapFrom(s => s.NumberOfSeats ?? 0))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.StatusName))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note));

            CreateMap<Application.Models.Orders.OrderStatus, DTOs.Responses.OrderStatusDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty));

            CreateMap<OrderLine, DTOs.Responses.OrderDetailDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.OrderID, o => o.MapFrom(s => s.OrderId))
                .ForMember(d => d.MenuID, o => o.MapFrom(s => s.MenuId))
                .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
                .ForMember(d => d.PriceListID, o => o.MapFrom(s => s.PriceListId))
                .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.CreatedBy ?? 0))
                .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate ?? default))
                .ForMember(d => d.CancelledBy, o => o.MapFrom(s => s.CancelledBy))
                .ForMember(d => d.CancelledDate, o => o.MapFrom(s => s.CancelledDate))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note))
                .ForMember(d => d.KindOfHotpot1ID, o => o.MapFrom(s => s.KindOfHotpot1Id))
                .ForMember(d => d.KindOfHotpot2ID, o => o.MapFrom(s => s.KindOfHotpot2Id))
                .ForMember(d => d.SizeID, o => o.MapFrom(s => s.SizeId))
                .ForMember(d => d.Menu, o => o.MapFrom(s => s.Menu))
                .ForMember(d => d.Size, o => o.MapFrom(s => s.Size));

            CreateMap<Application.Models.Orders.Menu, DTOs.Responses.MenuDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code ?? string.Empty))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty))
                .ForMember(d => d.MenuGroupName, o => o.MapFrom(s => s.MenuGroupName))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.StatusName))
                .ForMember(d => d.UnitName, o => o.MapFrom(s => s.UnitName))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note));

            CreateMap<Application.Models.Orders.Size, DTOs.Responses.SizeDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name ?? string.Empty))
                .ForMember(d => d.RankIndex, o => o.MapFrom(s => s.RankIndex ?? 0));

            // Tables empty endpoint
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

