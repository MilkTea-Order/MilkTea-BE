using MilkTea.Application.Features.Orders.Models.Dtos;
using MilkTea.Domain.Orders.Enums;

namespace MilkTea.Application.Features.Orders.Abstractions
{
    public interface IOrderQuery
    {
        /// <summary>
        /// Checks asynchronously if the specified table is available.
        /// </summary>
        /// <param name="tableId">The identifier of the table to check.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if the table is available;
        /// otherwise, false.</returns>
        Task<bool> IsTableAvailable(int tableId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of orders with optional filtering and sorting.
        /// </summary>
        /// <param name="orderBy">Specifies the sorting order for the returned orders.</param>
        /// <param name="status">Filters orders by their status if provided.</param>
        /// <param name="dayAgo">Limits orders to those created within the specified number of days ago.</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of order DTOs.</returns>
        //Task<List<OrderDto>> GetOrdersAsync(int orderBy, int? status, int? dayAgo, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of orders with optional filtering by date range.
        /// </summary>
        /// <param name="actionBy">Filter orders by the person who performed the action (CreatedBy for Unpaid, PaymentedBy for NotCollected, ActionBy for Paid, CancelledBy for Cancelled).</param>
        /// <param name="status">Filters orders by their status.</param>
        /// <param name="fromDate">Filters by action date (CreatedDate for Unpaid, PaymentedDate for NotCollected, ActionDate for Paid, CancelledDate for Cancelled).</param>
        /// <param name="toDate">Filters by action date (CreatedDate for Unpaid, PaymentedDate for NotCollected, ActionDate for Paid, CancelledDate for Cancelled).</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of order DTOs.</returns>
        Task<List<OrderDto>> GetOrdersAsync(int actionBy, OrderStatus status, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a report of orders grouped by date with statistics per payment method.
        /// </summary>
        /// <param name="actionBy">Filter by the person who performed the action (nullable for all).</param>
        /// <param name="orderStatus">Filter by order status.</param>
        /// <param name="fromDate">Filter by action date range (date field depends on status).</param>
        /// <param name="toDate">Filter by action date range (date field depends on status).</param>
        /// <param name="paymentMethod">Filter by payment method (only applicable for NotCollected and Paid).</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the order report DTO.</returns>
        Task<ReportOrderDto> GetOrderReportAsync(int actionBy, OrderStatus orderStatus, DateTime? fromDate, DateTime? toDate, string? paymentMethod, CancellationToken cancellationToken = default);
    }
}
