using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Users.Entities;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Identity;

/// <summary>
/// Entity Framework Core implementation of <see cref="IEmployeeRepository"/>.
/// Provides data access operations for Employee aggregate using EF Core.
/// </summary>
public class EmployeeRepository(AppDbContext context) : IEmployeeRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc/>
    /// <remarks>
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// </remarks>
    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Retrieves all employees from the database.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// Consider adding pagination or filtering if the dataset is large.
    /// </remarks>
    public async Task<List<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .AsNoTracking()
            .ToListAsync();
    }


    /// <inheritdoc/>
    /// <remarks>
    /// Checks email uniqueness excluding the specified employee ID.
    /// Uses AsNoTracking() for read-only queries.
    /// Compares against Email Value Object's Value property.
    /// </remarks>
    public async Task<bool> IsEmailExistAsync(string email, int excludeEmployeeId)
    {
        return await _context.Employees
            .AsNoTracking()
            .AnyAsync(e => !e.Email.IsEmpty && e.Email.Value == email && e.Id != excludeEmployeeId);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Checks cell phone uniqueness excluding the specified employee ID.
    /// Uses AsNoTracking() for read-only queries.
    /// Compares against PhoneNumber Value Object's Value property.
    /// </remarks>
    public async Task<bool> IsCellPhoneExistAsync(string cellPhone, int excludeEmployeeId)
    {
        return await _context.Employees
            .AsNoTracking()
            .AnyAsync(e => !e.CellPhone.IsEmpty && e.CellPhone.Value == cellPhone && e.Id != excludeEmployeeId);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Gets an employee by ID with change tracking enabled.
    /// Entity is tracked by EF Core change tracker for updates.
    /// Use this method when you need to modify and save the entity.
    /// </remarks>
    public async Task<Employee?> GetByIdForUpdateAsync(int id)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id);
    }
}
