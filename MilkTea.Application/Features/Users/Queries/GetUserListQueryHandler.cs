using MilkTea.Application.Features.Users.Abstractions.Queries;
using MilkTea.Application.Features.Users.Model.Results;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Users.Queries
{
    public class GetUserListQuery : IQuery<GetUserListResult> { }
    public class GetUserListQueryHandler(IUserQuery userQuery) : IQueryHandler<GetUserListQuery, GetUserListResult>
    {
        private readonly IUserQuery _vUserQuery = userQuery;

        public async Task<GetUserListResult> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var result = new GetUserListResult();
            var users = await _vUserQuery.GetUserListAsync(cancellationToken);
            result.Users = users;
            return result;
        }
    }
}
