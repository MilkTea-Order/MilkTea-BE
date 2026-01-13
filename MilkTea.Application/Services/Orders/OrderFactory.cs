using MilkTea.Application.Commands.Orders;
using MilkTea.Application.Models.Orders;
using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Respositories.Configs;
using MilkTea.Domain.Respositories.Orders;

namespace MilkTea.Application.Services.Orders
{
    public class OrderFactory(IOrderRepository orderRepository,
            IPriceListRepository priceListRepository,
            IStatusOfOrderRepository statusOfOrderRepository,
            IDenifitionRepository denifitionRepository)
    {
        private readonly IOrderRepository _vOrderRepository = orderRepository;
        private readonly IPriceListRepository _vPriceListRepository = priceListRepository;
        private readonly IStatusOfOrderRepository _vStatusOfOrderRepository = statusOfOrderRepository;
        private readonly IDenifitionRepository _vDenifitionRepository = denifitionRepository;



        public async Task<PriceList> GetActivePriceList()
            => await _vPriceListRepository.GetActivePriceListAsync()
               ?? throw new InvalidOperationException("Active PriceList not found");

        public async Task<StatusOfOrder> GetPendingStatus()
            => await _vStatusOfOrderRepository.GetPendingStatusAsync()
               ?? throw new InvalidOperationException("Pending Order Status not found");

        public async Task<Order> CreateOrder(
            CreateOrderCommand command,
            StatusOfOrder pendingStatus)
        {
            var now = DateTime.UtcNow;
            var codePrefixDefinition = await _vDenifitionRepository.GetCodePrefixBill();
            if (codePrefixDefinition is null)
            {
                throw new InvalidDataException("Not exist code prefix for bill! Need create code prefix for bill");
            }
            var countOrder = await _vOrderRepository.GetTotalOrdersCountInDateAsync(now.Date);
            var order = new Order
            {
                DinnerTableID = command.DinnerTableID,
                OrderBy = command.OrderedBy,
                OrderDate = now,
                CreatedBy = command.CreatedBy,
                CreatedDate = now,
                StatusOfOrderID = pendingStatus.ID,
                BillNo = $"{codePrefixDefinition.Value}{now:yyyyMMdd}{command.CreatedBy}{countOrder + 1}"
            };
            if (command.Note is not null)
            {
                order.Note = command.Note;
                order.AddNoteBy = command.CreatedBy;
                order.AddNoteDate = now;
            }
            return await _vOrderRepository.CreateOrderAsync(order);
        }

        public async Task CreateOrderDetails(
            Order order,
            IEnumerable<OrderItemValidation> validatedItems,
            int priceListId)
        {
            foreach (var item in validatedItems)
            {
                await _vOrderRepository.CreateOrderDetailAsync(new OrdersDetail
                {
                    OrderID = order.ID,
                    CreatedBy = order.CreatedBy,
                    MenuID = item.Menu!.ID,
                    SizeID = item.Item!.SizeID,
                    Quantity = item.Item.Quantity,
                    Price = item.Price!.Value,
                    PriceListID = priceListId,
                    Note = item.Item.Note,
                    CreatedDate = DateTime.UtcNow
                });
            }
        }
    }
}
