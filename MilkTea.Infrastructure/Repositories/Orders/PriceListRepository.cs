using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Constants.Enums;
using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Orders
{
    public class PriceListRepository(AppDbContext context) : IPriceListRepository
    {
        private readonly AppDbContext _vContext = context;

        public async Task<PriceList?> GetActivePriceListAsync()
        {
            return await _vContext.PriceList
                .AsNoTracking()
                .Include(pl => pl.Currency)
                .Where(pl => pl.StatusOfPriceListID == (int)PriceListStatus.Active)
                .OrderByDescending(pl => pl.ID)
                .FirstOrDefaultAsync();
        }

        public async Task<decimal?> GetPriceAsync(int priceListId, int menuId, int sizeId)
        {
            var priceDetail = await _vContext.PriceListDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(pld =>
                    pld.PriceListID == priceListId &&
                    pld.MenuID == menuId &&
                    pld.SizeID == sizeId);

            return priceDetail?.Price;
        }

        public async Task<Dictionary<int, decimal>> GetPricesForMenuAsync(int priceListId, int menuId)
        {
            var priceDetails = await _vContext.PriceListDetail
                .AsNoTracking()
                .Where(pld => pld.PriceListID == priceListId && pld.MenuID == menuId)
                .ToListAsync();
            return priceDetails.ToDictionary(pd => pd.SizeID, pd => pd.Price);
        }
    }
}