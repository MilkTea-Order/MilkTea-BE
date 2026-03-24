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
        /// <param name="orderBy">Specifies the sorting order for the returned orders.</param>
        /// <param name="status">Filters orders by their status if provided.</param>
        /// <param name="fromDate">Filters orders created on or after this date if provided.</param>
        /// <param name="toDate">Filters orders created on or before this date if provided.</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of order DTOs.</returns>
        Task<List<OrderDto>> GetOrdersAsync(int orderBy, OrderStatus status, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default);

        Task<ReportOrderDto> GetOrderReportAsync(int? orderBy, OrderStatus OrderStatusId, DateTime? FromDate, DateTime? ToDate, string? PaymentMethod, CancellationToken cancellationToken = default);
    }
}
