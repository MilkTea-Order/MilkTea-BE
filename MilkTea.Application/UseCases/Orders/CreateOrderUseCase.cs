using MilkTea.Application.Commands.Orders;
using MilkTea.Application.Models.Orders;
using MilkTea.Application.Results.Orders;
using MilkTea.Application.Services.Orders;
using MilkTea.Application.Ports.Identity;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories;

namespace MilkTea.Application.UseCases.Orders
{
    public class CreateOrderUseCase(
        IUnitOfWork unitOfWork,
        OrderRequestValidator requestValidator,
        OrderItemValidator itemValidator,
        OrderStockService stockService,
        OrderFactory orderFactory,
        OrderPricingService pricingService,
        ICurrentUser currentUser)
    {
        private readonly IUnitOfWork _vUnitOfWork = unitOfWork;
        private readonly OrderRequestValidator _vRequestValidator = requestValidator;
        private readonly OrderItemValidator _vItemValidator = itemValidator;
        private readonly OrderStockService _vStockService = stockService;
        private readonly OrderFactory _vOrderFactory = orderFactory;
        private readonly OrderPricingService _vPricingService = pricingService;
        private readonly ICurrentUser _currentUser = currentUser;

        public async Task<CreateOrderResult> Execute(CreateOrderCommand command)
        {
            var result = new CreateOrderResult();
            var createdBy = _currentUser.UserId;
            var orderedBy = command.OrderedBy ?? createdBy;

            // Validate request
            var requestError = await _vRequestValidator.Validate(command);
            if (requestError != null) return SendMessageError(result, requestError.Code, requestError.Fields);

            var activePriceList = await _vOrderFactory.GetActivePriceList();
            if (activePriceList is null) throw new InvalidDataException("ACTIVE_PRICE_LIST NOT EXIST!");


            var pendingOrderStatus = await _vOrderFactory.GetPendingStatus();
            if (pendingOrderStatus is null) throw new InvalidDataException("PENDING_STATUS_ORDER IS NOT EXIST!");

            if (command.Items == null || command.Items.Count == 0)
            {
                return SendMessageError(result, ErrorCode.E0036, "Items");
            }

            var validatedItems = new List<OrderItemValidation>();

            foreach (var item in command.Items)
            {
                var validation = await _vItemValidator.Validate(item, activePriceList.ID);

                if (validation.HasError) return SendMessageError(result, validation.Error!.Code, validation.Error.Fields);

                validatedItems.Add(validation);
            }
            // Check stock availability for all order items
            var stockError = await _vStockService.CheckAvailability(validatedItems);
            if (stockError != null) return SendMessageError(result, stockError.Code, stockError.Fields);

            await _vUnitOfWork.BeginTransactionAsync();

            try
            {
                var order = await _vOrderFactory.CreateOrder(command, pendingOrderStatus, createdBy, orderedBy);
                await _vOrderFactory.CreateOrderDetails(order, validatedItems, activePriceList.ID);
                order.TotalAmount = _vPricingService.CalculateTotalAmount(validatedItems);
                result.OrderDate = order.OrderDate;
                result.OrderID = order.ID;
                result.BillNo = order.BillNo;
                result.TotalAmount = order.TotalAmount;
                result.Items = validatedItems.Select(static item => new OrderItemResult
                {
                    MenuID = item.Item!.MenuID,
                    MenuName = item.Menu!.Name,
                    SizeID = item.Item.SizeID,
                    SizeName = item.Size!.Name,
                    Quantity = item.Item.Quantity,
                    Price = (decimal)item.Price!,
                    TotalPrice = (decimal)(item.Item.Quantity * item.Price),
                }).ToList();

                await _vUnitOfWork.CommitAsync();

                return result;
            }
            catch (Exception)
            {
                await _vUnitOfWork.RollbackAsync();
                return SendMessageError(result, ErrorCode.E9999, "CreateOrder");
            }
        }

        private CreateOrderResult SendMessageError(
            CreateOrderResult result,
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
