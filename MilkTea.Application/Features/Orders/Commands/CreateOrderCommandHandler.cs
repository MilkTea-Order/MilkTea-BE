using MediatR;
using MilkTea.Application.Features.Orders.Commands;
using MilkTea.Application.Models.Errors;
using MilkTea.Application.Models.Orders;
using MilkTea.Application.Ports.Users;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Domain.Configuration.Repositories;
using MilkTea.Domain.Inventory.Repositories;
using MilkTea.Domain.Orders.Entities;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.Orders.ValueObjects;
using MilkTea.Domain.Pricing.Enums;
using MilkTea.Domain.Pricing.Repositories;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.SharedKernel.Repositories;

namespace MilkTea.Application.Features.Orders.Commands;

public sealed class CreateOrderCommandHandler(
    IUnitOfWork unitOfWork,
    IOrderRepository orderRepository,
    ICurrentUser currentUser,
    IDinnerTableRepository dinnerTableRepository,
    IMenuRepository menuRepository,
    IPriceListRepository priceListRepository,
    ISizeRepository sizeRepository,
    IWarehouseRepository warehouseRepository,
    IDefinitionRepository definitionRepository) : IRequestHandler<CreateOrderCommand, CreateOrderResult>
{
    private const int PriceListId = (int)PriceListStatus.Active;

    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var result = new CreateOrderResult();
        var createdBy = currentUser.UserId;
        var orderedBy = command.OrderedBy ?? createdBy;

        var requestError = await ValidateRequest(command, cancellationToken);
        if (requestError != null) return SendError(result, requestError.Code, requestError.Fields);

        var validatedItems = new List<OrderItemValidation>();
        foreach (var item in command.Items!)
        {
            var validation = await ValidateItem(item, cancellationToken);
            if (validation.HasError)
                return SendErrorItem(result, validation.Error!.Code, validation.Error.Fields ?? [], item.MenuID, item.SizeID);
            validatedItems.Add(validation);
        }

        var stockErrors = await CheckStockAvailability(validatedItems, cancellationToken);
        if (stockErrors.Count > 0) return SendErrorMaterial(result, ErrorCode.ValidationFailed, stockErrors);

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var now = DateTime.UtcNow;
            var codePrefix = await definitionRepository.GetCodePrefixBill()
                ?? throw new InvalidOperationException("Not exist code prefix for bill. Need create code prefix for bill.");
            if (string.IsNullOrWhiteSpace(codePrefix.Value))
                throw new InvalidOperationException("Code prefix Value for bill is missing.");

            var count = await orderRepository.GetTotalOrdersCountInDateAsync(now.Date);
            var billNo = BillNo.Create(codePrefix.Value!, now, createdBy, count + 1);

            var order = Order.Create(
                billNo: billNo,
                dinnerTableId: command.DinnerTableID,
                orderBy: orderedBy,
                createdBy: createdBy,
                note: command.Note);

            foreach (var v in validatedItems)
            {
                var menuItem = MenuItem.Of(
                    menuId: v.Menu!.Id,
                    sizeId: v.Item!.SizeID,
                    price: v.Price!.Value,
                    priceListId: PriceListId,
                    kindOfHotpot1Id: v.Item.KindOfHotpotIDs?.Count > 0 ? v.Item.KindOfHotpotIDs[0] : null,
                    kindOfHotpot2Id: v.Item.KindOfHotpotIDs?.Count > 1 ? v.Item.KindOfHotpotIDs[1] : null);

                order.CreateOrderItem(
                    menuItem: menuItem,
                    quantity: v.Item.Quantity,
                    createdBy: createdBy,
                    note: v.Item.Note);
            }

            order.FinalizeAndPublishCreated();

            await orderRepository.AddAsync(order, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            result.OrderDate = order.OrderDate;
            result.OrderID = order.Id;
            result.BillNo = order.BillNo.Value;
            result.TotalAmount = order.TotalAmount;
            result.Items = validatedItems.Select(v => new OrderItemResult
            {
                MenuID = v.Item!.MenuID,
                MenuName = v.Menu!.Name,
                SizeID = v.Item.SizeID,
                SizeName = v.Size!.Name,
                Quantity = v.Item.Quantity,
                Price = (decimal)v.Price!,
                TotalPrice = (decimal)(v.Item.Quantity * v.Price),
            }).ToList();

            return result;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.ValidationFailed, "CreateOrder");
        }
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

    private static CreateOrderResult SendErrorMaterial(CreateOrderResult result, string errorCode, List<(int menuId, int sizeId, List<string> materialNames)> items)
    {
        result.ResultData.Add(errorCode, items.Select(x => string.Join(",", x.materialNames)).ToList());
        if (items.Count == 1)
        {
            result.ResultData.AddMeta("menuId", items[0].menuId);
            result.ResultData.AddMeta("sizeId", items[0].sizeId);
        }
        return result;
    }

    private async Task<ValidationError?> ValidateRequest(CreateOrderCommand command, CancellationToken ct)
    {
        if (await dinnerTableRepository.GetByIdAsync(command.DinnerTableID) == null)
            return ValidationError.InvalidData(nameof(command.DinnerTableID));

        if (command.OrderedBy.HasValue && command.OrderedBy.Value <= 0)
            return ValidationError.InvalidData(nameof(command.OrderedBy));

        if (command.Items == null || command.Items.Count == 0)
            return ValidationError.InvalidData(nameof(command.Items));

        return null;
    }

    private async Task<OrderItemValidation> ValidateItem(OrderItemCommand item, CancellationToken ct)
    {
        var v = new OrderItemValidation(item);

        var menu = await menuRepository.GetMenuByIDAndAvaliableAsync(item.MenuID);
        if (menu == null) return v.SetError(ValidationError.InvalidData(nameof(item.MenuID)));

        var price = await priceListRepository.GetPriceAsync(PriceListId, item.MenuID, item.SizeID);
        if (price == null) return v.SetError(ValidationError.InvalidData(nameof(item.SizeID)));

        if (item.Quantity <= 0) return v.SetError(ValidationError.InvalidData(nameof(item.Quantity)));

        var recipe = await warehouseRepository.GetRecipeAsync(item.MenuID, item.SizeID);
        if (recipe == null || recipe.Count == 0)
            return v.SetError(ValidationError.NotExist(nameof(item.SizeID), nameof(item.MenuID)));

        var size = await sizeRepository.GetByIdAsync(item.SizeID);
        return v.SetSuccess(menu, size!, price.Value, recipe);
    }

    private async Task<List<(int menuId, int sizeId, List<string> materialNames)>> CheckStockAvailability(List<OrderItemValidation> items, CancellationToken ct)
    {
        var required = new Dictionary<int, decimal>();
        var materialToItems = new Dictionary<int, List<OrderItemValidation>>();

        foreach (var it in items)
        {
            foreach (var r in it.Recipe!)
            {
                var qty = r.Quantity * it.Item!.Quantity;
                required[r.MaterialID] = required.GetValueOrDefault(r.MaterialID) + qty;
                if (!materialToItems.ContainsKey(r.MaterialID))
                    materialToItems[r.MaterialID] = new List<OrderItemValidation>();
                materialToItems[r.MaterialID].Add(it);
            }
        }

        var stock = await warehouseRepository.GetMaterialStockAsync(required.Keys.ToList());
        var materials = await warehouseRepository.GetMaterialsAsync(required.Keys.ToList());

        var insufficient = required
            .Where(r => stock.GetValueOrDefault(r.Key, 0) < r.Value)
            .Select(r => r.Key)
            .ToList();

        if (insufficient.Count == 0) return [];

        var menuSizeToNames = new Dictionary<(int MenuId, int SizeId), List<string>>();
        foreach (var mid in insufficient)
        {
            var name = materials.GetValueOrDefault(mid, $"MaterialID:{mid}");
            if (!materialToItems.TryGetValue(mid, out var affected)) continue;

            foreach (var it in affected)
            {
                if (it.Menu == null || it.Size == null) continue;
                var key = (it.Menu.Id, it.Size.Id);
                if (!menuSizeToNames.ContainsKey(key)) menuSizeToNames[key] = new List<string>();
                if (!menuSizeToNames[key].Contains(name)) menuSizeToNames[key].Add(name);
            }
        }

        return menuSizeToNames.Select(kv => (kv.Key.MenuId, kv.Key.SizeId, kv.Value)).ToList();
    }
}

