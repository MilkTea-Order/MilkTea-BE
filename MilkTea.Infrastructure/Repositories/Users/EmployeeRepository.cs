using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Users.Entities;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Users;

/// <summary>
/// Entity Framework Core implementation of <see cref="IEmployeeRepository"/>.
/// Provides data access operations for Employee aggregate using EF Core.
/// </summary>
public class EmployeeRepository(AppDbContext context) : IEmployeeRepository
{
    private readonly AppDbContext _vContext = context;

    /// <inheritdoc/>
    public async Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _vContext.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _vContext.Employees
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }


    /// <inheritdoc/>
    public async Task<bool> IsEmailExistAsync(string email, int excludeEmployeeId, CancellationToken cancellationToken = default)
    {
        return await _vContext.Employees
         .AsNoTracking()
         .AnyAsync(e =>
             e.Id != excludeEmployeeId &&
             e.Email.Value == email,
             cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> IsCellPhoneExistAsync(string cellPhone, int excludeEmployeeId, CancellationToken cancellationToken = default)
    {
        return await _vContext.Employees
            .AsNoTracking()
            .AnyAsync(e =>
                      e.Id != excludeEmployeeId &&
                      e.CellPhone.Value == cellPhone,
                      cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Employee?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _vContext.Employees
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Employee?> GetByIdWithGenderAndPositionAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _vContext.Employees
            .AsNoTracking()
            .Include(e => e.Gender)
            .Include(e => e.Position)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
