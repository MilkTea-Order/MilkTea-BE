using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Ports.Catalog;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Configuration.Repositories;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.Orders.ValueObjects;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands;

public sealed class CreateOrderCommandHandler(
    IOrderingUnitOfWork orderingUnitOfWork,
    IConfigurationUnitOfWork configurationUnitOfWork,
    ICatalogQuery catalogSalesQuery,
    ICurrentUser currentUser) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var result = new CreateOrderResult();
        var createdBy = currentUser.UserId;
        var orderedBy = command.OrderedBy ?? createdBy;

        var validatedItems = new List<ValidatedOrderItemDto>();
        foreach (var item in command.Items!)
        {
            var quote = await catalogSalesQuery.ValidateAndQuoteAsync(
                                                                command.DinnerTableID,
                                                                item.MenuID,
                                                                item.SizeID,
                                                                item.Quantity,
                                                                cancellationToken);
            if (!quote.IsValid)
            {
                var mapped = MapValidationError(quote);

                Console.WriteLine(quote.ErrorCode);
                if (quote.ErrorCode!.Equals("TABLE"))
                {
                    return SendError(result, mapped.errorCode, mapped.fields);
                }
                else
                {
                    return SendErrorItem(result, mapped.errorCode, mapped.fields, item.MenuID, item.SizeID);
                }
            }
            validatedItems.Add(new ValidatedOrderItemDto(item, quote));
        }

        await orderingUnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var now = DateTime.UtcNow;
            // throw => not created order successfully
            var codePrefix = await configurationUnitOfWork.Definitions.GetCodePrefixBill()
                ?? throw new InvalidOperationException("Bill number prefix is not configured.");
            if (string.IsNullOrWhiteSpace(codePrefix.Value))
                throw new InvalidOperationException("Bill number prefix value is missing.");

            var count = await orderingUnitOfWork.Orders.GetTotalOrdersCountInDateAsync(now.Date);
            var billNo = BillNo.Create(codePrefix.Value!, now, createdBy, count + 1);

            var order = Domain.Orders.Entities.Order.Create(
                billNo: billNo,
                dinnerTableId: command.DinnerTableID,
                orderBy: orderedBy,
                createdBy: createdBy,
                note: command.Note);
            foreach (var v in validatedItems)
            {
                var menuItem = MenuItem.Of(
                    menuId: v.OriginalItem.MenuID,
                    sizeId: v.OriginalItem.SizeID,
                    price: v.UnitPrice,
                    priceListId: v.PriceListId,
                    kindOfHotpot1Id: v.OriginalItem.KindOfHotpotIDs?.Count > 0 ? v.OriginalItem.KindOfHotpotIDs[0] : null,
                    kindOfHotpot2Id: v.OriginalItem.KindOfHotpotIDs?.Count > 1 ? v.OriginalItem.KindOfHotpotIDs[1] : null);
                order.CreateOrderItem(
                    menuItem: menuItem,
                    quantity: v.OriginalItem.Quantity,
                    createdBy: createdBy,
                    note: v.OriginalItem.Note);
            }
            await orderingUnitOfWork.Orders.AddAsync(order, cancellationToken);
            await orderingUnitOfWork.CommitTransactionAsync(cancellationToken);
            result.OrderDate = order.OrderDate;
            result.OrderID = order.Id;
            result.BillNo = order.BillNo.Value;
            result.TotalAmount = order.TotalAmount;
            return result;
        }
        catch
        {
            await orderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E9999, "CreateOrder");
        }
    }



    private static (string errorCode, string[] fields) MapValidationError(SalesValidationResult quote)
    {
        Console.WriteLine(quote.ErrorCode);
        return quote.ErrorCode switch
        {
            "TABLE" => (ErrorCode.E0036, ["dinnerTableId"]),
            "MENU_SIZE" => (ErrorCode.E0036, ["menu"]),
            "PRICE" => (ErrorCode.E0036, ["price"]),
            "QUANTITY" => (ErrorCode.E0036, ["quantity"]),
            _ => (ErrorCode.E0036, ["dinnerTableId", "menuID", "sizeID"])
        };
    }

    private static CreateOrderResult SendError(CreateOrderResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }

    private static CreateOrderResult SendErrorItem(CreateOrderResult result, string errorCode, string[] fields, int menuId, int sizeId)
    {
        if (fields is { Length: > 0 })
            result.ResultData.Add(errorCode, fields.ToList());
        result.ResultData.AddMeta("menuId", menuId);
        result.ResultData.AddMeta("sizeId", sizeId);
        return result;
    }

    private sealed class ValidatedOrderItemDto(OrderItemCommand originalItem, SalesValidationResult quote)
    {
        public OrderItemCommand OriginalItem { get; } = originalItem;
        public decimal UnitPrice { get; } = quote.UnitPrice!.Value;
        public int PriceListId { get; } = quote.PriceListId!.Value;
    }
}


//private static ValidationError? ValidateRequest(CreateOrderCommand command)
//{
//    if (command.Items.Any(i => i.MenuID <= 0))
//        return ValidationError.InvalidData(nameof(OrderItemCommand.MenuID));

//    if (command.Items.Any(i => i.SizeID <= 0))
//        return ValidationError.InvalidData(nameof(OrderItemCommand.SizeID));

//    if (command.Items.Any(i => i.Quantity <= 0))
//        return ValidationError.InvalidData(nameof(OrderItemCommand.Quantity));
//    return null;
//}