using MilkTea.Domain.Orders.Entities;
using MilkTea.Domain.Orders.Enums;

namespace MilkTea.Domain.Orders.Repositories;

/// <summary>
/// Repository interface for order-related data operations.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Adds an order (aggregate with details) to the context.
    /// </summary>
    Task AddAsync(Order order, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an order.
    /// </summary>
    Task<bool> UpdateAsync(Order order);

    /// <summary>
    /// Gets an order by its ID.
    /// </summary>
    Task<Order?> GetOrderByIdAsync(int orderId);

    /// <summary>
    /// Gets an order by its ID with order items (Can update).
    /// </summary>
    Task<Order?> GetOrderByIdWithItemsAsync(int orderId);

    /// <summary>
    /// Gets the total count of orders created on a specific date.
    /// </summary>
    Task<int> GetTotalOrdersCountInDateAsync(DateTime? date);

    /// <summary>
    /// Gets orders filtered by order creator and optional status.
    /// </summary>
    Task<List<Order>> GetOrdersByOrderByAndStatusWithItemsAsync(int orderBy, OrderStatus? status);

    /// <summary>
    /// Gets an order with details by ID and cancellation status.
    /// </summary>
    Task<Order?> GetOrderDetailByIDAndStatus(int orderID, bool? isCancelled);

    /// <summary>
    /// Gets all order item IDs for a specific order.
    /// </summary>
    Task<List<int>> GetOrderItemIdsByOrderIdAsync(int orderId);

    /// <summary>
    /// Checks if an order item is cancelled.
    /// </summary>
    Task<bool> IsOrderItemCancelledAsync(int orderItemId);

    /// <summary>
    /// Check if a dinner table is currently in use.
    /// </summary>
    /// <param name="dinnerTableId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>True if the dinner table is currently in use; otherwise, false.</returns>
    Task<bool> HadUsing(int dinnerTableId, CancellationToken cancellationToken = default);

}
