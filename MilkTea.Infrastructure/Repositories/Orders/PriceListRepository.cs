using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Constants.Enums;
using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Orders
{
    public class PriceListRepository(AppDbContext context) : IPriceListRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<PriceList?> GetActivePriceListAsync()
        {
            return await _context.PriceList
                .AsNoTracking()
                .Where(pl => pl.StatusOfPriceListID == (int)PriceListStatus.Active)
                .OrderByDescending(pl => pl.ID)
                .FirstOrDefaultAsync();
        }

        public async Task<decimal?> GetPriceAsync(int priceListId, int menuId, int sizeId)
        {
            var priceDetail = await _context.PriceListDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(pld =>
                    pld.PriceListID == priceListId &&
                    pld.MenuID == menuId &&
                    pld.SizeID == sizeId);

            return priceDetail?.Price;
        }
    }
}