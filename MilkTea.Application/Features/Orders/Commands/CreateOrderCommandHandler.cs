using FluentValidation;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Orders.Models.Dtos;
using MilkTea.Application.Features.Orders.Models.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Catalog.Table.Enums;
using MilkTea.Domain.Common.Constants;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.Orders.ValueObjects;
using Shared.Abstractions.CQRS;
using Shared.Extensions;

namespace MilkTea.Application.Features.Orders.Commands;
public class CreateOrderCommand : ICommand<CreateOrderResult>
{
    public int DinnerTableId { get; set; }
    public List<OrderItemCommand> Items { get; set; } = new();
    public int? OrderedBy { get; set; }
    public string? Note { get; set; }
}

public class OrderItemCommand
{
    public int MenuId { get; set; }
    public int SizeId { get; set; }
    public int Quantity { get; set; }
    public List<int>? ToppingIDs { get; set; }
    public List<int>? KindOfHotpotIDs { get; set; }
    public string? Note { get; set; }
}

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.DinnerTableId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName(nameof(CreateOrderCommand.DinnerTableId));
        RuleFor(x => x.Items)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName(nameof(CreateOrderCommand.Items));
        RuleFor(x => x.OrderedBy)
            .GreaterThan(0)
            .When(x => x.OrderedBy.HasValue)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName(nameof(CreateOrderCommand.OrderedBy));
    }
}


public sealed class CreateOrderCommandHandler(IOrderUnitOfWork orderingUnitOfWork,
                                                ICatalogService catalogService,
                                                IIdentifyServicePorts currentUser) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    private readonly IOrderUnitOfWork _vOrderingUnitOfWork = orderingUnitOfWork;
    private readonly ICatalogService _vCatalogService = catalogService;
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        CreateOrderResult result = new();
        var createdBy = currentUser.UserId;
        var orderedBy = command.OrderedBy ?? createdBy;

        // Validate dinnerTable
        var hadUsing = await _vOrderingUnitOfWork.Orders.HadUsing(command.DinnerTableId, cancellationToken);
        if (hadUsing)
        {
            return SendError(result, ErrorCode.E0042, nameof(command.DinnerTableId));
        }
        var table = await _vCatalogService.GetTableAsync(command.DinnerTableId, cancellationToken);
        if (table is null || table.Status.Id != (int)TableStatus.InUsing)
        {
            return SendError(result, ErrorCode.E0042, nameof(command.DinnerTableId));
        }

        // Validate quantity of each items
        var hasError = false;
        foreach (var item in command.Items)
        {
            if (item.Quantity <= 0)
            {
                if (!hasError)
                {
                    result = SendError(result, ErrorCode.E0036, "Quantity");
                    hasError = true;
                }
                AddItemMeta("Quantity", result, item);
            }
        }
        if (hasError)
        {
            return result;
        }

        //Validate each items
        var menuSizePairs = command.Items.Select(i => (MenuID: i.MenuId, SizeID: i.SizeId)).Distinct().ToList();
        var canPayMap = await _vCatalogService.CanPayBatch(menuSizePairs, cancellationToken);
        foreach (var item in command.Items)
        {
            var key = (MenuID: item.MenuId, SizeID: item.SizeId);
            if (!canPayMap.TryGetValue(key, out var info) || !info.CanPay)
            {
                if (!hasError)
                {
                    hasError = true;
                    result = SendError(result, ErrorCode.E0036, "Menu");
                }
                AddItemMeta("Menu", result, item);
            }
        }
        if (hasError)
        {
            return result;
        }

        await _vOrderingUnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var order = Domain.Orders.Entities.OrderEntity.Create(
                dinnerTableId: command.DinnerTableId,
                orderBy: orderedBy,
                createdBy: createdBy,
                note: command.Note);
            foreach (var item in command.Items)
            {
                var priceInfo = canPayMap[(item.MenuId, item.SizeId)];
                var menuItem = MenuItem.Of(
                    menuId: item.MenuId,
                    sizeId: item.SizeId,
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
            result.Order = new OrderDto
            {
                OrderId = order.Id,
                OrderBy = order.OrderBy,
                OrderDate = order.OrderDate,
                CreatedBy = order.CreatedBy,
                CreatedDate = order.CreatedDate,
                Note = order.Note,
                TotalAmount = order.GetTotalAmount(),
                Status = new OrderStatusDto
                {
                    Id = (int)order.Status,
                    Name = order.Status.GetDescription()
                },
                DinnerTable = new TableDto
                {
                    Id = table.Id,
                    Code = table.Code,
                    Name = table.Name,
                    Position = table.Position,
                    NumberOfSeats = table.NumberOfSeats,
                    StatusId = table.Status.Id,
                    StatusName = table.Status.Name,
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
    private static void AddItemMeta(string key, CreateOrderResult result, dynamic item)
    {
        var i = new
        {
            key = key,
            menuId = item.MenuID,
            sizeId = item.SizeID
        };
        result.ResultData.AddMeta(key, i);
    }
}