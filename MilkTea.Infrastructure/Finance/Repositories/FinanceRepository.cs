using MilkTea.Domain.Finance.Entities;
using MilkTea.Domain.Finance.Repositoties;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Finance.Repositories
{
    public class FinanceRepository(AppDbContext context) : IFinanceRepository
    {
        private readonly AppDbContext _vContext = context;
        public async Task AddAsync(CollectAndSpendEntity transaction, CancellationToken cancellationToken = default)
        {
            await _vContext.CollectAndSpends.AddAsync(transaction, cancellationToken);
        }
    }
}
