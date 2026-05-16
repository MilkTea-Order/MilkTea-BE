using MilkTea.Application.Features.Auth.Models.Dtos;

namespace MilkTea.Application.Features.Auth.Abstractions.Queries;

public interface IPermissionQuery
{
    Task<List<PermissionDto>> GetPermissionsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}
