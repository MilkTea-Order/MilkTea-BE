using MilkTea.Domain.Finance.Entities;

namespace MilkTea.Domain.Finance.Repositoties
{
    public interface IFinanceRepository
    {
        /// <summary>
        /// Adds a transaction (collect or spend) to the context.
        /// </summary>
        Task AddAsync(CollectAndSpendEntity transaction, CancellationToken cancellationToken = default);
    }
}
