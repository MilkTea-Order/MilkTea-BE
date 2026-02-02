using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            var response = _vMapper.Map<CreateOrderResponseDto>(result);

            return SendSuccess(response);
        }

        [Authorize]
        [HttpGet]
        public async Task<ResponseDto> GetOrdersByOrderByAndStatus([FromQuery] int? statusId)
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
        public async Task<ResponseDto> GetOrderDetailByIDAndStatusID([FromRoute] int orderId, [FromQuery] bool? isCancelled)
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
        [HttpPatch("{orderId:int}/cancel")]
        public async Task<ResponseDto> CancelOrder([FromRoute] int orderId, [FromBody] CancelOrderRequestDto request)
        {
            var command = new CancelOrderCommand
            {
                OrderID = orderId,
                CancelNote = request.CancelNote
            };

            var result = await _vSender.Send(command);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }

            var response = _vMapper.Map<CancelOrderResponseDto>(result);

            return SendSuccess(response);
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

            var response = _vMapper.Map<CancelOrderDetailsResponseDto>(result);

            return SendSuccess(response);
        }
    }
}
