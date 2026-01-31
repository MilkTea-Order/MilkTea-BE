using MediatR;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Models.Orders;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.SharedKernel.Constants;

namespace MilkTea.Application.Features.Orders.Queries;

public sealed class GetOrderDetailByIdAndStatusQueryHandler(
    IOrderingUnitOfWork orderingUnitOfWork) : IRequestHandler<GetOrderDetailByIdAndStatusQuery, GetOrderDetailByIDAndStatusResult>
{
    public async Task<GetOrderDetailByIDAndStatusResult> Handle(GetOrderDetailByIdAndStatusQuery query, CancellationToken cancellationToken)
    {
        var result = new GetOrderDetailByIDAndStatusResult();

        if (query.OrderId <= 0)
        {
            return SendError(result, ErrorCode.E0036, nameof(query.OrderId));
        }

        var order = await orderingUnitOfWork.Orders.GetOrderDetailByIDAndStatus(query.OrderId, query.IsCancelled);

        if (order is null)
        {
            return SendError(result, ErrorCode.E0001, nameof(query.OrderId));
        }

        result.Order = new OrderDetail
        {
            OrderId = order.Id,
            DinnerTableId = order.DinnerTableId,
            OrderDate = order.OrderDate,
            OrderBy = order.OrderBy,
            CreatedDate = order.CreatedDate,
            CreatedBy = order.CreatedBy,
            StatusId = (int)order.Status,
            Note = order.Note,
            TotalAmount = order.TotalAmount,
            OrderDetails = order.OrderItems
                .Where(item => query.IsCancelled == null || item.IsCancelled == query.IsCancelled)
                .Select(item => new OrderLine
                {
                    Id = item.Id,
                    OrderId = order.Id,
                    MenuId = item.MenuItem.MenuId,
                    SizeId = item.MenuItem.SizeId,
                    Quantity = item.Quantity,
                    Price = item.MenuItem.Price,
                    PriceListId = item.MenuItem.PriceListId,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    CancelledBy = item.CancelledBy,
                    CancelledDate = item.CancelledDate,
                    Note = item.Note,
                    KindOfHotpot1Id = item.MenuItem.KindOfHotpot1Id,
                    KindOfHotpot2Id = item.MenuItem.KindOfHotpot2Id
                }).ToList()
        };

        return result;
    }

    private static GetOrderDetailByIDAndStatusResult SendError(GetOrderDetailByIDAndStatusResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
