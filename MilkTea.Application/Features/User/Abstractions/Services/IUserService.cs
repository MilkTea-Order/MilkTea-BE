namespace MilkTea.Application.Features.User.Abstractions.Services;

public interface IUserService
{
    /// <summary>
    /// Gets a user by their email address.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user ID if found, otherwise null.</returns>
    Task<int?> GetUserIdByEmailAsync(string email, CancellationToken cancellationToken = default);
}
