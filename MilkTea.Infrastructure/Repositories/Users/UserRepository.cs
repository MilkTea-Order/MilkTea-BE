using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Users.Entities;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Identity;

/// <summary>
/// Repository implementation for user operations.
/// </summary>
public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc/>
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <inheritdoc/>
    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .Include(u => u.UserPermissions)
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }

    /// <inheritdoc/>
    public async Task<User?> GetWithEmployeeAsync(int userId)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.Employee)
                .ThenInclude(e => e!.Gender)
            .Include(u => u.Employee)
                .ThenInclude(e => e!.Position)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        return await _context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc/>
    public async Task<User> CreateAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }
}
