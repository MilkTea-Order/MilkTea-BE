using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Domain.Respositories.Orders
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<OrdersDetail> CreateOrderDetailAsync(OrdersDetail orderDetail);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<int> GetTotalOrdersCountInDateAsync(DateTime? date);
        Task<List<Dictionary<string, object?>>> GetOrdersByOrderByAndStatusIDAsync(int orderBy, int? statusId);
        Task<Order?> GetOrderDetailByIDAndStatus(int orderID, bool isCancelled);
        Task<bool> CancelOrderAsync(Order order);
        Task<bool> CancelOrderDetailsAsync(int orderId, int cancelledBy, DateTime cancelledDate);
        Task<bool> CancelOrderDetailAsync(int orderDetailId, int cancelledBy, DateTime cancelledDate);
        Task<bool> CancelSpecificOrderDetailsAsync(List<int> orderDetailIds, int cancelledBy, DateTime cancelledDate);
        Task<List<int>> GetOrderDetailIdsByOrderIdAsync(int orderId);
        Task<bool> IsOrderDetailCancelledAsync(int orderDetailId);
    }
}