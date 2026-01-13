using AutoMapper;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.API.RestfulAPI.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            // Map Order entity to GetOrderDetailByIDAndStatusResponseDto
            CreateMap<Order, GetOrderDetailByIDAndStatusResponseDto>()
                .ForMember(dest => dest.OrderID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.StatusID, opt => opt.MapFrom(src => src.StatusOfOrderID))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount ?? 0))
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrdersDetails));

            // Map OrdersDetail entity to OrderDetailDto
            CreateMap<OrdersDetail, OrderDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.PriceListID, opt => opt.MapFrom(src => src.PriceListID ?? 0));
        }
    }
}