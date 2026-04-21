using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Auth.Abstractions.Queries;
using MilkTea.Application.Features.Configuration.Abstractions.Services;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Features.Auth.Queries;

public class AuthQuery(
    AppDbContext context,
    IConfigurationService configurationService) : IAuthQuery
{
    private readonly AppDbContext _vContext = context;
    private readonly IConfigurationService _vConfigurationService = configurationService;

    public async Task<int?> GetUserIdByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken)
    {
        return await _vContext.Users
            .AsNoTracking()
            .Where(u => u.EmployeeID == employeeId)
            .Select(u => (int?)u.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
