using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Catalog.Services
{
    public class TableServices(AppDbContext context) : ITableServices
    {
        private readonly AppDbContext _vContext = context;

        public async Task<bool> IsTableInUsing(int tableId, CancellationToken cancellationToken = default)
        {
            return await _vContext.Tables.AsNoTracking()
                                    .Where(x => x.Id == tableId &&
                                           x.Status == Domain.Catalog.Enums.TableStatus.InUsing)
                                    .AnyAsync(cancellationToken);
        }
    }
}
