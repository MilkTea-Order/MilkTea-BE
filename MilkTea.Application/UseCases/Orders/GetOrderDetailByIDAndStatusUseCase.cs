using MilkTea.Application.DTOs.Orders;
using MilkTea.Application.Queries.Orders;
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
        public async Task<GetOrderDetailByIDAndStatusResult> Execute(GetOrderDetailByIdAndStatusQuery query)
        {
            GetOrderDetailByIDAndStatusResult result = new();
            if (query.OrderId <= 0)
            {
                return SendMessageError(result, ErrorCode.E0036, nameof(query.OrderId));
            }

            var order = await _vOrderRepository.GetOrderDetailByIDAndStatus(query.OrderId, query.IsCancelled);

            if (order == null)
            {
                return SendMessageError(result, ErrorCode.E0001, nameof(query.OrderId));
            }

            result.Order = new OrderDetailDto
            {
                OrderId = order.ID,
                DinnerTableId = order.DinnerTableID,
                OrderDate = order.OrderDate,
                OrderBy = order.OrderBy,
                CreatedDate = order.CreatedDate,
                CreatedBy = order.CreatedBy,
                StatusId = order.StatusOfOrderID,
                Note = order.Note,
                TotalAmount = order.TotalAmount ?? 0m,
                DinnerTable = order.DinnerTable == null ? null : new DinnerTableDto
                {
                    Id = order.DinnerTable.ID,
                    Code = order.DinnerTable.Code,
                    Name = order.DinnerTable.Name,
                    Position = order.DinnerTable.Position,
                    NumberOfSeats = order.DinnerTable.NumberOfSeats,
                    StatusId = order.DinnerTable.StatusOfDinnerTableID,
                    StatusName = order.DinnerTable.StatusOfDinnerTable?.Name,
                    Note = order.DinnerTable.Note
                },
                Status = order.StatusOfOrder == null ? null : new OrderStatusDto
                {
                    Id = order.StatusOfOrder.ID,
                    Name = order.StatusOfOrder.Name
                },
                OrderDetails = (order.OrdersDetails ?? new List<MilkTea.Domain.Entities.Orders.OrdersDetail>())
                    .Select(static d => new OrderLineDto
                    {
                        Id = d.ID,
                        OrderId = d.OrderID,
                        MenuId = d.MenuID,
                        SizeId = d.SizeID,
                        Quantity = d.Quantity,
                        Price = d.Price,
                        PriceListId = d.PriceListID ?? 0,
                        CreatedBy = d.CreatedBy,
                        CreatedDate = d.CreatedDate,
                        CancelledBy = d.CancelledBy,
                        CancelledDate = d.CancelledDate,
                        Note = d.Note,
                        KindOfHotpot1Id = d.KindOfHotpot1ID,
                        KindOfHotpot2Id = d.KindOfHotpot2ID,
                        Menu = d.Menu == null ? null : new MenuDto
                        {
                            Id = d.Menu.ID,
                            Code = d.Menu.Code,
                            Name = d.Menu.Name,
                            MenuGroupName = d.Menu.MenuGroup?.Name,
                            StatusName = d.Menu.Status?.Name,
                            UnitName = d.Menu.Unit?.Name,
                            Note = d.Menu.Note
                        },
                        Size = d.Size == null ? null : new SizeDto
                        {
                            Id = d.Size.ID,
                            Name = d.Size.Name,
                            RankIndex = d.Size.RankIndex
                        }
                    }).ToList()
            };
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
