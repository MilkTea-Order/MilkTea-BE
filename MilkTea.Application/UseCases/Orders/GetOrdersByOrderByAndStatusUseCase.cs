using MilkTea.Application.DTOs.Orders;
using MilkTea.Application.Ports.Identity;
using MilkTea.Application.Queries.Orders;
using MilkTea.Application.Results.Orders;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories.Orders;

namespace MilkTea.Application.UseCases.Orders
{
    public class GetOrdersByOrderByAndStatusUseCase(
                                        IOrderRepository orderRepository,
                                        IStatusOfOrderRepository statusOfOrderRepository,
                                        ICurrentUser currentUser)
    {
        private readonly IOrderRepository _vOrderRepository = orderRepository;
        private readonly IStatusOfOrderRepository _vStatusOfOrderRepository = statusOfOrderRepository;
        private readonly ICurrentUser _currentUser = currentUser;

        public async Task<GetOrdersByOrderByAndStatusResult> Execute(GetOrdersByOrderByAndStatusQuery query)
        {
            GetOrdersByOrderByAndStatusResult result = new();
            // Validate status ID if provided
            if (query.StatusId.HasValue)
            {
                bool statusExists = await _vStatusOfOrderRepository.ExistsAsync(query.StatusId.Value);
                if (!statusExists) return SendMessageError(result, ErrorCode.E0036, nameof(query.StatusId));
            }
            var orders = await _vOrderRepository.GetOrdersByOrderByAndStatusIDAsync(_currentUser.UserId, query.StatusId);
            result.Orders = orders.Select(static o => new OrderDto
            {
                OrderId = o.ID,
                DinnerTableId = o.DinnerTableID,
                OrderDate = o.OrderDate,
                OrderBy = o.OrderBy,
                CreatedDate = o.CreatedDate,
                CreatedBy = o.CreatedBy,
                StatusId = o.StatusOfOrderID,
                Note = o.Note,
                TotalAmount = o.TotalAmount ?? 0m
            }).ToList();

            return result;
        }

        private GetOrdersByOrderByAndStatusResult SendMessageError(
           GetOrdersByOrderByAndStatusResult result,
           string errorCode,
           params string[] values)
        {
            if (values != null && values.Length > 0)
            {
                result.ResultData.Add(errorCode, values.ToList());
            }
            return result;
        }
    }
}
