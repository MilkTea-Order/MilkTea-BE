using MediatR;
using MilkTea.Application.Features.Users.Results;

namespace MilkTea.Application.Features.Users.Queries;

public sealed class GetUserProfileQuery : IRequest<GetUserProfileResult>
{
    // Intentionally empty: user context is resolved via ICurrentUser.
}

