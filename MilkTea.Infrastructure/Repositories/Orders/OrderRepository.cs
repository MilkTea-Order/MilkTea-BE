using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Infrastructure.Persistence;
using MilkTea.Shared.Extensions;

namespace MilkTea.Infrastructure.Repositories.Orders
{
    public class OrderRepository(AppDbContext context) : IOrderRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<OrdersDetail> CreateOrderDetailAsync(OrdersDetail orderDetail)
        {
            await _context.OrdersDetail.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
            return orderDetail;
        }

        public async Task<List<Dictionary<string, object?>>> GetOrdersByOrderByAndStatusIDAsync(int orderBy, int statusId)
        {
            var row = await _context.Orders
                .Where(o => o.OrderBy == orderBy && o.StatusOfOrderID == statusId)
                .ToListAsync();
            return row.ToDictList();
        }
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.ID == orderId);
        }

        public async Task<int> GetTotalOrdersCountInDateAsync(DateTime? date)
        {
            DateTime startDate = date?.Date ?? DateTime.Now.Date;
            var endDate = startDate.AddDays(1).AddTicks(-1);

            return await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate < endDate)
                .CountAsync();
        }

        public async Task<Order?> GetOrderDetailByIDAndStatus(int orderId, bool isCancelled)
        {
            return await _context.Orders
                .Include(o => o.OrdersDetails.Where(od => isCancelled
                    ? od.CancelledBy != null && od.CancelledDate != null
                    : od.CancelledBy == null && od.CancelledDate == null))
                .Where(o => o.ID == orderId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<int>> GetOrderDetailIdsByOrderIdAsync(int orderId)
        {
            return await _context.OrdersDetail
                .Where(od => od.OrderID == orderId)
                .Select(od => od.ID)
                .ToListAsync();
        }
        //public async Task<Order?> GetOrderByIDAndStatus(int orderId, int statusId)
        //{
        //    return await _context.Orders
        //        .Include(o => o.OrdersDetails)
        //        .Where(o => o.ID == orderId && o.StatusOfOrderID == statusId)
        //        .FirstOrDefaultAsync();
        //}

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CancelOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CancelOrderDetailsAsync(int orderId, int cancelledBy, DateTime cancelledDate)
        {
            var orderDetails = await _context.OrdersDetail
                .Where(od => od.OrderID == orderId)
                .ToListAsync();

            foreach (var detail in orderDetails)
            {
                detail.CancelledBy = cancelledBy;
                detail.CancelledDate = cancelledDate;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CancelOrderDetailAsync(int orderDetailId, int cancelledBy, DateTime cancelledDate)
        {
            var detail = await _context.OrdersDetail
                .FirstOrDefaultAsync(od => od.ID == orderDetailId && od.CancelledBy == null);

            if (detail is null)
                return false;

            detail.CancelledBy = cancelledBy;
            detail.CancelledDate = cancelledDate;

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> CancelSpecificOrderDetailsAsync(List<int> orderDetailIds, int cancelledBy, DateTime cancelledDate)
        {
            try
            {
                var details = await _context.OrdersDetail
                    .Where(od => orderDetailIds.Contains(od.ID) && od.CancelledBy == null)
                    .ToListAsync();

                if (details.Count == 0)
                    return false;

                foreach (var detail in details)
                {
                    detail.CancelledBy = cancelledBy;
                    detail.CancelledDate = cancelledDate;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsOrderDetailCancelledAsync(int orderDetailId)
        {
            var orderDetail = await _context.OrdersDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(od => od.ID == orderDetailId);

            if (orderDetail is null)
                return false;

            return orderDetail.CancelledBy.HasValue || orderDetail.CancelledDate.HasValue;
        }
    }
}