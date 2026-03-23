namespace MilkTea.Application.Features.Users.Abstractions.Services
{
    public interface IUserServices
    {
        Task<bool> isExist(int userID, CancellationToken cancellationToken = default);
    }
}
