using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Orders.Entities;
using MilkTea.Domain.Orders.Enums;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Orders;

/// <summary>
/// Repository implementation for order-related data operations.
/// </summary>
public class OrderRepository(AppDbContext context) : IOrderRepository
{
    private readonly AppDbContext _vContext = context;

    /// <inheritdoc/>
    public async Task AddAsync(OrderEntity order, CancellationToken cancellationToken = default)
    {
        await _vContext.Orders.AddAsync(order, cancellationToken);
    }

    /// <inheritdoc/>
    public void Remove(OrderEntity order)
    {
        _vContext.Orders.Remove(order);
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(OrderEntity order)
    {
        _vContext.Orders.Update(order);
        return await _vContext.SaveChangesAsync() > 0;
    }

    /// <inheritdoc/>
    public async Task<List<OrderEntity>> GetOrdersByOrderByAndStatusWithItemsAsync(int orderBy, OrderStatus? status)
    {
        var query = _vContext.Orders.AsNoTracking().Where(o => o.OrderBy == orderBy);

        if (status.HasValue)
            query = query.Where(o => o.Status == status.Value);

        return await query
        .AsSplitQuery()
        .Include(o => o.OrderItems)
        .OrderByDescending(o => o.Id)
        .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<OrderEntity?> GetOrderByIdAsync(int orderId)
    {
        return await _vContext.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    /// <inheritdoc/>
    public async Task<OrderEntity?> GetOrderByIdWithItemsAsync(int orderId)
    {
        return await _vContext.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    /// <inheritdoc/>
    public async Task<int> GetTotalOrdersCountInDateAsync(DateTime? date)
    {
        DateTime startDate = date?.Date ?? DateTime.Now.Date;
        var endDate = startDate.AddDays(1).AddTicks(-1);

        return await _vContext.Orders
            .Where(o => o.OrderDate >= startDate && o.OrderDate < endDate)
            .CountAsync();
    }

    /// <inheritdoc/>
    public async Task<OrderEntity?> GetOrderDetailByIDAndStatus(int orderId, bool? isCancelled)
    {
        var query = _vContext.Orders
            .Where(o => o.Id == orderId)
            .Include(o => o.OrderItems.Where(oi =>
                isCancelled == null
                    ? true
                    : isCancelled.Value
                        ? oi.CancelledBy != null && oi.CancelledDate != null
                        : oi.CancelledBy == null && oi.CancelledDate == null
            ));

        return await query.FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<List<int>> GetOrderItemIdsByOrderIdAsync(int orderId)
    {
        return await _vContext.OrderItems
            .Where(oi => oi.OrderId == orderId)
            .Select(oi => oi.Id)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> IsOrderItemCancelledAsync(int orderItemId)
    {
        var orderItem = await _vContext.OrderItems
            .AsNoTracking()
            .FirstOrDefaultAsync(oi => oi.Id == orderItemId);

        if (orderItem is null)
            return false;

        return orderItem.IsCancelled;
    }

    /// <inheritdoc/>
    public async Task<bool> HadUsing(int dinnerTableId, CancellationToken cancellationToken = default)
    {
        return await _vContext.Orders
            .AsNoTracking()
            .AnyAsync(o => o.DinnerTableId == dinnerTableId && o.Status == OrderStatus.Unpaid, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<OrderEntity?> GetOrderByTableAndStatusWithItemsAsync(int tableId, OrderStatus? status, CancellationToken cancellationToken = default)
    {
        return await _vContext.Orders
                        .Where(o => o.DinnerTableId == tableId && (!status.HasValue || o.Status == status.Value))
                        .Include(o => o.OrderItems)
                        .OrderByDescending(x => x.CreatedDate)
                        .FirstOrDefaultAsync(cancellationToken);
    }
}
