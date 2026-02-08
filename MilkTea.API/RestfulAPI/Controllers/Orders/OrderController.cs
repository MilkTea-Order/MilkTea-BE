using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Order.Responses;
using MilkTea.API.RestfulAPI.DTOs.Orders.Requests;
using MilkTea.API.RestfulAPI.DTOs.Orders.Responses;
using MilkTea.API.RestfulAPI.DTOs.Requests;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Features.Orders.Commands;
using MilkTea.Application.Features.Orders.Queries;

namespace MilkTea.API.RestfulAPI.Controllers.Orders
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController(ISender sender,
                                IMapper mapper) : BaseController
    {
        private readonly ISender _vSender = sender;
        private readonly IMapper _vMapper = mapper;

        [Authorize]
        [HttpGet]
        public async Task<ResponseDto> GetOrdersByOrderByAndStatus([FromQuery] int statusId = 1)
        {
            var query = new GetOrdersByOrderByAndStatusQuery
            {
                StatusId = statusId
            };

            var result = await _vSender.Send(query);

            if (result.ResultData.HasData) return SendError(result.ResultData);

            var response = result.Orders.Select(order => _vMapper.Map<GetOrdersByOrderByAndStatusResponseDto>(order)).ToList();
            return SendSuccess(response);
        }

        [Authorize]
        [HttpGet("{orderId:int}")]
        public async Task<ResponseDto> GetOrderDetailByIDAndStatusID([FromRoute] int orderId, [FromQuery] bool isCancelled = false)
        {
            var query = new GetOrderDetailByIdAndStatusQuery
            {
                OrderId = orderId,
                IsCancelled = isCancelled
            };

            var result = await _vSender.Send(query);

            if (result.ResultData.HasData) return SendError(result.ResultData);

            var response = _vMapper.Map<GetOrderDetailByIDAndStatusResponseDto>(result.Order);
            return SendSuccess(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<ResponseDto> CreateOrder([FromBody] CreateOrderRequestDto request)
        {
            var command = new CreateOrderCommand
            {
                DinnerTableID = request.DinnerTableID,
                OrderedBy = request.OrderByID,
                Note = request.Note,
                Items = request.Items.Select(i => new OrderItemCommand
                {
                    MenuID = i.MenuID,
                    SizeID = i.SizeID,
                    Quantity = i.Quantity,
                    ToppingIDs = i.ToppingIDs,
                    KindOfHotpotIDs = i.KindOfHotpotIDs,
                    Note = i.Note
                }).ToList()
            };
            var result = await _vSender.Send(command);

            if (result.ResultData.HasData) return SendError(result.ResultData);

            var response = _vMapper.Map<CreateOrderResponseDto>(result.Order);

            return SendSuccess(response);
        }

        [Authorize]
        [HttpPatch("{orderId:int}/add-items")]
        public async Task<ResponseDto> AddOrderDetail([FromRoute] int orderId, [FromBody] AddOrderDetailRequestDto request)
        {
            var command = new AddOrderDetailCommand
            {
                OrderID = orderId,
                Items = request.Items.Select(i => new OrderItemCommand
                {
                    MenuID = i.MenuID,
                    SizeID = i.SizeID,
                    Quantity = i.Quantity,
                    ToppingIDs = i.ToppingIDs,
                    KindOfHotpotIDs = i.KindOfHotpotIDs,
                    Note = i.Note
                }).ToList()
            };
            var result = await _vSender.Send(command);
            if (result.ResultData.HasData) return SendError(result.ResultData);
            return SendSuccess();
        }

        [Authorize]
        [HttpPatch("{orderId:int}/cancel")]
        public async Task<ResponseDto> CancelOrder([FromRoute] int orderId)
        {
            var command = new CancelOrderCommand
            {
                OrderID = orderId,
            };

            var result = await _vSender.Send(command);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            return SendSuccess();
        }

        [Authorize]
        [HttpPatch("{orderId:int}/items/{orderDetailId:int}/cancel")]
        public async Task<ResponseDto> CancelOrderDetail([FromRoute] int orderId, [FromRoute] int orderDetailId)
        {
            var command = new CancelOrderDetailCommnad
            {
                OrderID = orderId,
                OrderDetailID = orderDetailId,
            };
            var result = await _vSender.Send(command);
            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            return SendSuccess();
        }

        [Authorize]
        [HttpPatch("{orderId:int}/items/cancel")]
        public async Task<ResponseDto> CancelOrderDetails([FromRoute] int orderId, [FromBody] CancelOrderDetailsRequestDto request)
        {
            var command = new CancelOrderDetailsCommand
            {
                OrderID = orderId,
                OrderDetailIDs = request.OrderDetailIDs,
            };

            var result = await _vSender.Send(command);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            return SendSuccess();
        }


        [Authorize]
        [HttpPatch("{orderId:int}/items/{orderDetailId:int}/update")]
        public async Task<ResponseDto> UpdateOrderDetail([FromRoute] int orderId, [FromRoute] int orderDetailId, [FromBody] UpdateOrderDetailRequestDto request)
        {
            var command = new UpdateOrderDetailCommand
            {
                OrderID = orderId,
                OrderDetailID = orderDetailId,
                Quantity = request.Quantity,
                Note = request.Note
            };
            var result = await _vSender.Send(command);
            if (result.ResultData.HasData) return SendError(result.ResultData);
            return SendSuccess();
        }
    }
}
