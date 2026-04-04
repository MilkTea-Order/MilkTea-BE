using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.User.Abstractions.Services;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Features.User.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<int?> GetUserIdByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        var normalizedEmail = email.Trim().ToLowerInvariant();

        var userId = await _context.Employees.AsNoTracking()
                                                .Where(e => e.Email.Value.ToLower() == normalizedEmail)
                                                .Select(e => e.Id)
                                                .FirstOrDefaultAsync(cancellationToken);

        return userId > 0 ? userId : null;
    }
}
