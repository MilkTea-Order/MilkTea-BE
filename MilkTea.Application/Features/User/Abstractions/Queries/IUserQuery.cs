using MilkTea.Application.Features.User.Model.Dtos;

namespace MilkTea.Application.Features.User.Abstractions.Queries
{
    public interface IUserQuery
    {
        Task<List<UserProfile>> GetUserListAsync(List<int> employeeIds, CancellationToken cancellationToken = default);
        Task<UserProfile?> GetUserProfileByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default);
        Task<int?> GetEmployeeIdByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
