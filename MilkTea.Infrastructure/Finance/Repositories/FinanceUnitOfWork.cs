using Microsoft.EntityFrameworkCore.Storage;
using MilkTea.Domain.Finance.Repositoties;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Finance.Repositories
{
    public class FinanceUnitOfWork(AppDbContext context,
                                IFinanceRepository financeRepository) : IFinanceUnitOfWork
    {
        private readonly AppDbContext _vContext = context;
        private IDbContextTransaction? _vTransaction;
        public IFinanceRepository Finance { get; } = financeRepository;

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _vTransaction = await _vContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _vContext.SaveChangesAsync(cancellationToken);
                if (_vTransaction != null)
                {
                    await _vTransaction.CommitAsync(cancellationToken);
                }

            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                if (_vTransaction != null)
                {
                    await _vTransaction.DisposeAsync();
                    _vTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_vTransaction != null)
            {
                await _vTransaction.RollbackAsync(cancellationToken);
                await _vTransaction.DisposeAsync();
                _vTransaction = null;
            }
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => _vContext.SaveChangesAsync(cancellationToken);
    }
}