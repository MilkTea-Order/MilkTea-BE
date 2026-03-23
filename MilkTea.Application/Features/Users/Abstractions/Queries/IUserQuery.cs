using MilkTea.Application.Features.Users.Model.Dtos;

namespace MilkTea.Application.Features.Users.Abstractions.Queries
{
    public interface IUserQuery
    {
        //Task<UserProfile?> GetUserProfileAsync(CancellationToken cancellationToken);
        Task<List<UserProfile>> GetUserListAsync(CancellationToken cancellationToken);
    }
}
