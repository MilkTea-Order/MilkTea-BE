using AutoMapper;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.API.RestfulAPI.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            // Mapping: Menu -> MenuDto
            CreateMap<Menu, MenuDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.ID))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.MenuGroupName, o => o.MapFrom(s => s.MenuGroup != null ? s.MenuGroup.Name : null))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status != null ? s.Status.Name : null))
                .ForMember(d => d.UnitName, o => o.MapFrom(s => s.Unit != null ? s.Unit.Name : null))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note));

            // Mapping: Size -> SizeDto
            CreateMap<Size, SizeDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.ID))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.RankIndex, o => o.MapFrom(s => s.RankIndex));

            CreateMap<OrdersDetail, OrderDetailDto>()
           .ForMember(d => d.Id, o => o.MapFrom(s => s.ID))
           .ForMember(d => d.OrderID, o => o.MapFrom(s => s.OrderID))
           .ForMember(d => d.MenuID, o => o.MapFrom(s => s.MenuID))
           .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity))
           .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
           // entity: int?  -> dto: int (bắt buộc) => cho default 0 nếu null
           .ForMember(d => d.PriceListID, o => o.MapFrom(s => s.PriceListID ?? 0))
           .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.CreatedBy))
           .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate))
           .ForMember(d => d.CancelledBy, o => o.MapFrom(s => s.CancelledBy))
           .ForMember(d => d.CancelledDate, o => o.MapFrom(s => s.CancelledDate))
           .ForMember(d => d.Note, o => o.MapFrom(s => s.Note))
           .ForMember(d => d.KindOfHotpot1ID, o => o.MapFrom(s => s.KindOfHotpot1ID))
           .ForMember(d => d.KindOfHotpot2ID, o => o.MapFrom(s => s.KindOfHotpot2ID))
           .ForMember(d => d.SizeID, o => o.MapFrom(s => s.SizeID))
           .ForMember(d => d.Menu, o => o.MapFrom(s => s.Menu))
           .ForMember(d => d.Size, o => o.MapFrom(s => s.Size));

            // Mapping: DinnerTable -> DinnerTableDto
            CreateMap<DinnerTable, DinnerTableDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.ID))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Position, o => o.MapFrom(s => s.Position))
                .ForMember(d => d.NumberOfSeats, o => o.MapFrom(s => s.NumberOfSeats))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.StatusOfDinnerTable != null ? s.StatusOfDinnerTable.Name : null))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note));

            // Mapping: StatusOfOrder -> OrderStatusDto
            CreateMap<StatusOfOrder, OrderStatusDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.ID))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name));

            // Cha: Order -> GetOrderDetailByIDAndStatusResponseDto
            CreateMap<Order, GetOrderDetailByIDAndStatusResponseDto>()
                .ForMember(d => d.OrderID, o => o.MapFrom(s => s.ID))
                .ForMember(d => d.DinnerTableID, o => o.MapFrom(s => s.DinnerTableID))
                .ForMember(d => d.OrderDate, o => o.MapFrom(s => s.OrderDate))
                .ForMember(d => d.OrderBy, o => o.MapFrom(s => s.OrderBy))
                .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate))
                .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.CreatedBy))
                .ForMember(d => d.StatusID, o => o.MapFrom(s => s.StatusOfOrderID))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note))
                .ForMember(d => d.TotalAmount, o => o.MapFrom(s => s.TotalAmount ?? 0m))
                .ForMember(d => d.DinnerTable, o => o.MapFrom(s => s.DinnerTable))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.StatusOfOrder))
                .ForMember(d => d.OrderDetails, o => o.MapFrom(s => s.OrdersDetails));

            // Mapping: Order -> GetOrdersByOrderByAndStatusResponseDto (cho API list orders)
            CreateMap<Order, GetOrdersByOrderByAndStatusResponseDto>()
                .ForMember(d => d.OrderID, o => o.MapFrom(s => s.ID))
                .ForMember(d => d.DinnerTableID, o => o.MapFrom(s => s.DinnerTableID))
                .ForMember(d => d.OrderDate, o => o.MapFrom(s => s.OrderDate))
                .ForMember(d => d.OrderBy, o => o.MapFrom(s => s.OrderBy))
                .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate))
                .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.CreatedBy))
                .ForMember(d => d.StatusID, o => o.MapFrom(s => s.StatusOfOrderID))
                .ForMember(d => d.Note, o => o.MapFrom(s => s.Note))
                .ForMember(d => d.TotalAmount, o => o.MapFrom(s => s.TotalAmount ?? 0m))
                .ForMember(d => d.DinnerTable, o => o.MapFrom(s => s.DinnerTable))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.StatusOfOrder))
                .ForMember(d => d.OrderDetails, o => o.MapFrom(s => new List<OrderDetailDto>()));
            // Mapping: DinnerTable -> GetTableEmptyResponseDto
            CreateMap<DinnerTable, GetTableEmptyResponseDto>()
                .ForMember(d => d.TableID, o => o.MapFrom(s => s.ID))
                .ForMember(d => d.TableCode, o => o.MapFrom(s => s.Code))
                .ForMember(d => d.TableName, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.NumberOfSeat, o => o.MapFrom(s => s.NumberOfSeats))
                .ForMember(d => d.TableNote, o => o.MapFrom(s => s.Note))
                .ForMember(d => d.StatusID, o => o.MapFrom(s => s.StatusOfDinnerTableID))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.StatusOfDinnerTable != null ? s.StatusOfDinnerTable.Name : null))
                .ForMember(d => d.UsingImg, o => o.MapFrom(s => s.UsingPicture != null ? $"data:image/png;base64,{Convert.ToBase64String(s.UsingPicture)}" : null))
                .ForMember(d => d.EmptyImg, o => o.MapFrom(s => s.EmptyPicture != null ? $"data:image/png;base64,{Convert.ToBase64String(s.EmptyPicture)}" : null));
        }
    }
}