using MilkTea.Application.Models.Orders;

namespace MilkTea.Application.Services.Orders
{
    public class OrderPricingService
    {
        public decimal CalculateTotalAmount(
            IEnumerable<OrderItemValidation> items)
        {
            decimal total = 0;

            foreach (var item in items)
            {
                total += item.Price!.Value * item.Item!.Quantity;
            }

            return total;
        }
    }

}
