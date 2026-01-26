using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Orders.Entities;
using MilkTea.Domain.Orders.Enums;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Ordering;

/// <summary>
/// Repository implementation for order-related data operations.
/// </summary>
public class OrderRepository(AppDbContext context) : IOrderRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc/>
    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        return await _context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc/>
    public async Task<OrderItem> CreateOrderItemAsync(OrderItem orderItem)
    {
        await _context.OrderItems.AddAsync(orderItem);
        return orderItem;
    }

    /// <inheritdoc/>
    public async Task<List<Order>> GetOrdersByOrderByAndStatusAsync(int orderBy, OrderStatus? status)
    {
        var query = _context.Orders.Where(o => o.OrderBy == orderBy);

        if (status.HasValue)
            query = query.Where(o => o.Status == status.Value);

        return await query.OrderByDescending(o => o.Id).ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    /// <inheritdoc/>
    public async Task<int> GetTotalOrdersCountInDateAsync(DateTime? date)
    {
        DateTime startDate = date?.Date ?? DateTime.Now.Date;
        var endDate = startDate.AddDays(1).AddTicks(-1);

        return await _context.Orders
            .Where(o => o.OrderDate >= startDate && o.OrderDate < endDate)
            .CountAsync();
    }

    /// <inheritdoc/>
    public async Task<Order?> GetOrderDetailByIDAndStatus(int orderId, bool? isCancelled)
    {
        var query = _context.Orders
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
        return await _context.OrderItems
            .Where(oi => oi.OrderId == orderId)
            .Select(oi => oi.Id)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> CancelOrderAsync(Order order)
    {
        _context.Orders.Update(order);
        return await _context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc/>
    public async Task<bool> CancelOrderItemsAsync(int orderId, int cancelledBy, DateTime cancelledDate)
    {
        var orderItems = await _context.OrderItems
            .Where(oi => oi.OrderId == orderId)
            .ToListAsync();

        foreach (var item in orderItems)
        {
            item.Cancel(cancelledBy);
        }

        return await _context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc/>
    public async Task<bool> CancelOrderItemAsync(int orderItemId, int cancelledBy, DateTime cancelledDate)
    {
        var item = await _context.OrderItems
            .FirstOrDefaultAsync(oi => oi.Id == orderItemId && oi.CancelledBy == null);

        if (item is null)
            return false;

        item.Cancel(cancelledBy);

        return await _context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc/>
    public async Task<bool> CancelSpecificOrderItemsAsync(List<int> orderItemIds, int cancelledBy, DateTime cancelledDate)
    {
        try
        {
            var items = await _context.OrderItems
                .Where(oi => orderItemIds.Contains(oi.Id) && oi.CancelledBy == null)
                .ToListAsync();

            if (items.Count == 0)
                return false;

            foreach (var item in items)
            {
                item.Cancel(cancelledBy);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> IsOrderItemCancelledAsync(int orderItemId)
    {
        var orderItem = await _context.OrderItems
            .AsNoTracking()
            .FirstOrDefaultAsync(oi => oi.Id == orderItemId);

        if (orderItem is null)
            return false;

        return orderItem.IsCancelled;
    }
}
