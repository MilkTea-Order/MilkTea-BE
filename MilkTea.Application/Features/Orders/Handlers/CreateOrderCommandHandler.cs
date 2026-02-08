using MilkTea.Application.Features.Catalog.Services;
using MilkTea.Application.Features.Orders.Commands;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Models.Orders;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.Orders.ValueObjects;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;
using Shared.Extensions;

namespace MilkTea.Application.Features.Orders.Handlers;

public sealed class CreateOrderCommandHandler(
    IOrderingUnitOfWork orderingUnitOfWork,
    ICatalogService catalogService,
    ICurrentUser currentUser) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    private readonly IOrderingUnitOfWork _vOrderingUnitOfWork = orderingUnitOfWork;
    private readonly ICatalogService _vCatalogService = catalogService;
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        CreateOrderResult result = new CreateOrderResult();
        var createdBy = currentUser.UserId;
        var orderedBy = command.OrderedBy ?? createdBy;

        // Validate dinnerTable
        var hadUsing = await _vOrderingUnitOfWork.Orders.HadUsing(command.DinnerTableID, cancellationToken);
        if (hadUsing)
        {
            return SendError(result, ErrorCode.E0036, "dinnerTableId");
        }
        var table = await _vCatalogService.GetTableAsync(command.DinnerTableID, cancellationToken);
        if (table is null || table.StatusId != (int)TableStatus.InUsing)
        {
            return SendError(result, ErrorCode.E0036, "dinnerTableId");
        }

        // Validate quantity of each items
        var hasError = false;
        foreach (var item in command.Items)
        {
            if (item.Quantity <= 0)
            {
                if (!hasError)
                {
                    result = SendError(result, ErrorCode.E0036, "quantity");
                    hasError = true;
                }
                AddItemMeta(result, item);
            }
        }
        if (hasError)
        {
            return result;
        }

        //Validate each items
        var menuSizePairs = command.Items.Select(i => (i.MenuID, i.SizeID)).Distinct().ToList();
        var canPayMap = await _vCatalogService.CanPayBatch(menuSizePairs, cancellationToken);
        foreach (var item in command.Items)
        {
            var key = (item.MenuID, item.SizeID);
            if (!canPayMap.TryGetValue(key, out var info) || !info.CanPay)
            {
                if (!hasError)
                {
                    hasError = true;
                    result = SendError(result, ErrorCode.E0001, "menu");
                }
                AddItemMeta(result, item);
            }
        }
        if (hasError)
        {
            return result;
        }

        await _vOrderingUnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var order = Domain.Orders.Entities.Order.Create(
                dinnerTableId: command.DinnerTableID,
                orderBy: orderedBy,
                createdBy: createdBy,
                note: command.Note);
            foreach (var item in command.Items)
            {
                var priceInfo = canPayMap[(item.MenuID, item.SizeID)];
                var menuItem = MenuItem.Of(
                    menuId: item.MenuID,
                    sizeId: item.SizeID,
                    priceListId: priceInfo.Data.PriceListID,
                    price: priceInfo.Data.Price,
                    kindOfHotpot1Id: item.KindOfHotpotIDs?.Count > 0 ? item.KindOfHotpotIDs[0] : null,
                    kindOfHotpot2Id: item.KindOfHotpotIDs?.Count > 1 ? item.KindOfHotpotIDs[1] : null);
                order.CreateOrderItem(
                    menuItem: menuItem,
                    quantity: item.Quantity,
                    createdBy: createdBy,
                    note: item.Note);
            }
            await _vOrderingUnitOfWork.Orders.AddAsync(order, cancellationToken);
            await _vOrderingUnitOfWork.CommitTransactionAsync(cancellationToken);
            result.Order = new Order
            {
                OrderId = order.Id,
                DinnerTableId = order.DinnerTableId,
                OrderBy = order.OrderBy,
                OrderDate = order.OrderDate,
                CreatedBy = order.CreatedBy,
                CreatedDate = order.CreatedDate,
                StatusId = (int)order.Status,
                Note = order.Note,
                TotalAmount = order.GetTotalAmount(),
                Status = new OrderStatus
                {
                    Id = (int)order.Status,
                    Name = order.Status.GetDescription()
                },
                DinnerTable = new Table
                {
                    Id = table.Id,
                    Code = table.Code,
                    Name = table.Name,
                    Position = table.Position,
                    NumberOfSeats = table.NumberOfSeats,
                    StatusId = table.StatusId,
                    StatusName = table.StatusName,
                    Note = table.Note,
                    UsingImg = table.UsingImg,
                }
            };
            return result;
        }
        catch
        {
            await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E9999, "CreateOrder");
        }
    }
    private static CreateOrderResult SendError(CreateOrderResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
    private static void AddItemMeta(CreateOrderResult result, dynamic item)
    {
        var i = new
        {
            menuId = item.MenuID,
            sizeId = item.SizeID
        };
        result.ResultData.AddMeta("InvalidItems", i);
    }
}