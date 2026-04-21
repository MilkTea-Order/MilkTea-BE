namespace MilkTea.Application.Features.Auth.Abstractions.Queries
{
    public interface IAuthQuery
    {
        Task<int?> GetUserIdByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default);
    }
}
