using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Requests;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Commands.Orders;
using MilkTea.Application.UseCases.Orders;
using MilkTea.Infrastructure.Authentication.JWT.Extensions;

namespace MilkTea.API.RestfulAPI.Controllers.Orders
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController(CreateOrderUseCase createOrderUseCase,
                                GetOrdersByOrderByAndStatusUseCase getOrdersByOrderByAndStatusUseCase,
                                GetOrderDetailByIDAndStatusUseCase getOrderDetailByIDAndStatusUseCase,
                                CancelOrderUseCase cancelOrderUseCase,
                                CancelOrderDetailsUseCase cancelOrderDetailsUseCase,
                                IMapper mapper) : BaseController
    {
        private readonly CreateOrderUseCase _vCreateOrderUseCase = createOrderUseCase;
        private readonly GetOrdersByOrderByAndStatusUseCase _vGetOrdersByOrderByAndStatusUseCase = getOrdersByOrderByAndStatusUseCase;
        private readonly GetOrderDetailByIDAndStatusUseCase _vGetOrderDetailByIDAndStatusUseCase = getOrderDetailByIDAndStatusUseCase;
        private readonly CancelOrderUseCase _vCancelOrderUseCase = cancelOrderUseCase;
        private readonly CancelOrderDetailsUseCase _vCancelOrderDetailsUseCase = cancelOrderDetailsUseCase;
        private readonly IMapper _vMapper = mapper;
        [Authorize]
        [HttpPost]
        public async Task<ResponseDto> CreateOrder([FromBody] CreateOrderRequestDto request)
        {
            var vUserID = User.GetUserId();
            if (!int.TryParse(vUserID, out var vUserIdInt))
            {
                return SendError();
            }
            var command = new CreateOrderCommand
            {
                DinnerTableID = request.DinnerTableID,
                CreatedBy = vUserIdInt,
                OrderedBy = request.OrderByID ?? vUserIdInt,
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
            var result = await _vCreateOrderUseCase.Execute(command);

            if (result.ResultData.HasData) return SendError(result.ResultData);

            var response = new CreateOrderResponseDto
            {
                OrderID = result.OrderID!.Value,
                BillNo = result.BillNo!,
                TotalAmount = result.TotalAmount!.Value,
                OrderDate = result.OrderDate!.Value,
                Items = result.Items.Select(i => new OrderItemResponse
                {
                    MenuID = i.MenuID,
                    MenuName = i.MenuName ?? string.Empty,
                    SizeID = i.SizeID,
                    SizeName = i.SizeName,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    TotalPrice = i.TotalPrice
                }).ToList()
            };

            return SendSuccess(response);
        }

        [Authorize]
        [HttpGet]
        public async Task<ResponseDto> GetOrdersByOrderByAndStatus([FromQuery] int? statusId)
        {
            var vUserID = User.GetUserId();
            if (!int.TryParse(vUserID, out var vUserIdInt))
            {
                return SendError();
            }
            var result = await _vGetOrdersByOrderByAndStatusUseCase.Execute(new GetOrdersByOrderByAndStatusCommand
            {
                OrderBy = vUserIdInt,
                StatusId = statusId
            });

            if (result.ResultData.HasData) return SendError(result.ResultData);

            var response = result.Orders.Select(order => _vMapper.Map<GetOrdersByOrderByAndStatusResponseDto>(order)).ToList();
            return SendSuccess(response);
        }

        [Authorize]
        [HttpGet("{orderId:int}")]
        public async Task<ResponseDto> GetOrderDetailByIDAndStatusID([FromRoute] int orderId, [FromQuery] bool? isCancelled)
        {

            var result = await _vGetOrderDetailByIDAndStatusUseCase.Execute(new GetOrderDetailByIDAndStatusCommand
            {
                OrderID = orderId,
                IsCancelled = isCancelled
            });

            if (result.ResultData.HasData) return SendError(result.ResultData);

            var response = _vMapper.Map<GetOrderDetailByIDAndStatusResponseDto>(result.Order);
            return SendSuccess(response);
        }

        [Authorize]
        [HttpPatch("{orderId:int}/cancel")]
        public async Task<ResponseDto> CancelOrder([FromRoute] int orderId, [FromBody] CancelOrderRequestDto request)
        {
            var vUserID = User.GetUserId();
            if (!int.TryParse(vUserID, out var vUserIdInt))
            {
                return SendError();
            }

            var command = new CancelOrderCommand
            {
                OrderID = orderId,
                CancelledBy = vUserIdInt,
                CancelNote = request.CancelNote
            };

            var result = await _vCancelOrderUseCase.Execute(command);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }

            var response = new CancelOrderResponseDto
            {
                OrderID = result.OrderID!.Value,
                BillNo = result.BillNo!,
                CancelledDate = result.CancelledDate!.Value
            };

            return SendSuccess(response);
        }

        [Authorize]
        [HttpPatch("{orderId:int}/items/cancel")]
        public async Task<ResponseDto> CancelOrderDetails([FromRoute] int orderId, [FromBody] CancelOrderDetailsRequestDto request)
        {
            var vUserID = User.GetUserId();
            if (!int.TryParse(vUserID, out var vUserIdInt))
            {
                return SendError();
            }

            var command = new CancelOrderDetailsCommand
            {
                OrderID = orderId,
                OrderDetailIDs = request.OrderDetailIDs,
                CancelledBy = vUserIdInt
            };

            var result = await _vCancelOrderDetailsUseCase.Execute(command);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }

            var response = new CancelOrderDetailsResponseDto
            {
                OrderID = result.OrderID!.Value,
                CancelledDetailIDs = result.CancelledDetailIDs,
                CancelledDate = result.CancelledDate!.Value
            };

            return SendSuccess(response);
        }
    }
}
