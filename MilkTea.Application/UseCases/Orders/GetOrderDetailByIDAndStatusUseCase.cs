using MilkTea.Application.Commands.Orders;
using MilkTea.Application.Results.Orders;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories.Orders;

namespace MilkTea.Application.UseCases.Orders
{
    public class GetOrderDetailByIDAndStatusUseCase(IOrderRepository orderRepository,
                                        IStatusOfOrderRepository statusOfOrderRepository)
    {
        private readonly IOrderRepository _vOrderRepository = orderRepository;
        private readonly IStatusOfOrderRepository _VStatusOfOrderRepository = statusOfOrderRepository;
        public async Task<GetOrderDetailByIDAndStatusResult> Execute(GetOrderDetailByIDAndStatusCommand command)
        {
            GetOrderDetailByIDAndStatusResult result = new();
            if (command.OrderID <= 0)
            {
                return SendMessageError(result, ErrorCode.E0036, nameof(command.OrderID));
            }

            var order = await _vOrderRepository.GetOrderDetailByIDAndStatus(command.OrderID, command.IsCancelled);

            if (order == null)
            {
                return SendMessageError(result, ErrorCode.E0001, nameof(command.OrderID));
            }

            result.Order = order;
            return result;
        }

        private GetOrderDetailByIDAndStatusResult SendMessageError(
           GetOrderDetailByIDAndStatusResult result,
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
