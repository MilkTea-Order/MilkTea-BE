using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.User.Abstractions.Services;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Features.User.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _vContext;

    public UserService(AppDbContext context)
    {
        _vContext = context;
    }

    /// <inheritdoc/>
    public async Task<int?> GetUserIdByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        var userId = await _vContext.Employees.AsNoTracking()
                                                .Where(e => e.Email.Value == email)
                                                .Select(e => e.Id)
                                                .FirstOrDefaultAsync(cancellationToken);

        return userId > 0 ? userId : null;
    }
}
