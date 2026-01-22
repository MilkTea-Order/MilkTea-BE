using MilkTea.Application.Commands.Orders;
using MilkTea.Application.Results.Orders;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories.Orders;

namespace MilkTea.Application.UseCases.Orders
{
    public class GetOrdersByOrderByAndStatusUseCase(
                                        IOrderRepository orderRepository,
                                        IStatusOfOrderRepository statusOfOrderRepository)
    {
        private readonly IOrderRepository _vOrderRepository = orderRepository;
        private readonly IStatusOfOrderRepository _vStatusOfOrderRepository = statusOfOrderRepository;
        public async Task<GetOrdersByOrderByAndStatusResult> Execute(GetOrdersByOrderByAndStatusCommand command)
        {
            GetOrdersByOrderByAndStatusResult result = new();
            if (command.OrderBy <= 0)
            {
                return SendMessageError(result, ErrorCode.E0036, nameof(command.OrderBy));
            }
            // Validate status ID if provided
            if (command.StatusId.HasValue)
            {
                bool statusExists = await _vStatusOfOrderRepository.ExistsAsync(command.StatusId.Value);
                if (!statusExists) return SendMessageError(result, ErrorCode.E0036, nameof(command.StatusId));
            }
            result.Orders = await _vOrderRepository.GetOrdersByOrderByAndStatusIDAsync(command.OrderBy, command.StatusId);

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
