using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Users.Entities;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Identity;

/// <summary>
/// Repository implementation for employee operations.
/// </summary>
public class EmployeeRepository(AppDbContext context) : IEmployeeRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc/>
    public async Task<EmployeeProfile?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    /// <inheritdoc/>
    public async Task<List<EmployeeProfile>> GetAllAsync()
    {
        return await _context.Employees
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(EmployeeProfile employee)
    {
        _context.Employees.Update(employee);
        return await _context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc/>
    public async Task<bool> IsEmailExistAsync(string email, int excludeEmployeeId)
    {
        return await _context.Employees
            .AsNoTracking()
            .AnyAsync(e => e.Email == email && e.Id != excludeEmployeeId);
    }

    /// <inheritdoc/>
    public async Task<bool> IsCellPhoneExistAsync(string cellPhone, int excludeEmployeeId)
    {
        return await _context.Employees
            .AsNoTracking()
            .AnyAsync(e => e.CellPhone == cellPhone && e.Id != excludeEmployeeId);
    }
}
