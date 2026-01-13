using Microsoft.EntityFrameworkCore.Storage;
using MilkTea.Domain.Respositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        private readonly AppDbContext _context = context;
        private IDbContextTransaction? _transaction;

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
