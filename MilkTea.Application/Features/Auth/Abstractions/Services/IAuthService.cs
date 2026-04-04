using MilkTea.Application.Features.Auth.Models.Dtos;

namespace MilkTea.Application.Features.Auth.Abstractions.Services
{
    public interface IAuthService
    {
        Task<bool> IsExist(int userID, CancellationToken cancellationToken = default);
        Task<AccountDto?> GetAccountByUserIdAsync(int userID, CancellationToken cancellationToken = default);
        Task<List<AccountDto>> GetAccountsAsync(CancellationToken cancellationToken = default);
    }
}
