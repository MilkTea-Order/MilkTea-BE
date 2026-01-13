using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Domain.Respositories.Orders
{
    public interface IPriceListRepository
    {
        Task<PriceList?> GetActivePriceListAsync();
        Task<decimal?> GetPriceAsync(int priceListId, int menuId, int sizeId);
    }
}