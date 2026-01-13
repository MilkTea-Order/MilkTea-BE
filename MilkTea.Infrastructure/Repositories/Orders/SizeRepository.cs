using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Orders
{
    public class SizeRepository(AppDbContext context) : ISizeRepository
    {
        private readonly AppDbContext _vContext = context;

        public async Task<Size?> GetSizeByIdAsync(int sizeId)
        {
            return await _vContext.Size
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.ID == sizeId);
        }
    }
}