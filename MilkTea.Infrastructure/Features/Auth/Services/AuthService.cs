using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Auth.Abstractions.Services;
using MilkTea.Application.Features.Auth.Models.Dtos;
using MilkTea.Infrastructure.Persistence;
using Shared.Extensions;
namespace MilkTea.Infrastructure.Features.Auth.Services
{
    public class AuthService(AppDbContext context) : IAuthService
    {
        private readonly AppDbContext _vContext = context;

        public async Task<AccountDto?> GetAccountByUserIdAsync(int userID, CancellationToken cancellationToken = default)
        {
            return await _vContext.Users.AsNoTracking().Select(u => new AccountDto
            {
                UserID = u.Id,
                EmployeeID = u.EmployeeID,
                Username = u.UserName.ToString(),
                StatusID = (int)u.Status,
                StatusName = u.Status.GetDescription()
            }).FirstOrDefaultAsync(u => u.UserID == userID, cancellationToken);
        }

        public async Task<bool> IsExist(int userID, CancellationToken cancellationToken = default)
        {
            return await _vContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userID, cancellationToken) is not null;
        }

        public async Task<List<AccountDto>> GetAccountsAsync(CancellationToken cancellationToken = default)
        {
            return await _vContext.Users.AsNoTracking().Select(u => new AccountDto
            {
                UserID = u.Id,
                EmployeeID = u.EmployeeID,
                Username = u.UserName.ToString(),
                StatusID = (int)u.Status,
                StatusName = u.Status.GetDescription()
            }).ToListAsync(cancellationToken);
        }
    }
}
