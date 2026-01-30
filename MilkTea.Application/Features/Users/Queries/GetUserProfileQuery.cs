using MediatR;
using MilkTea.Application.Features.Users.Results;

namespace MilkTea.Application.Features.Users.Queries;

public sealed class GetUserProfileQuery : IRequest<GetUserProfileResult>
{
}

