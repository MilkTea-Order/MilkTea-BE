using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Users.Abstractions.Queries;
using MilkTea.Application.Features.Users.Model.Dtos;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.User.Queries
{
    public class UserQuery(AppDbContext context) : IUserQuery
    {
        private readonly AppDbContext _vContext = context;

        public async Task<List<UserProfile>> GetUserListAsync(CancellationToken cancellationToken)
        {
            var query = _vContext.Users.AsNoTracking()
                                    .Join(_vContext.Employees, u => u.EmployeeID,
                                                e => e.Id,
                                                (u, e) => new { User = u, Employee = e })
                                    .Select(joined => new UserProfile
                                    {
                                        UserId = joined.User.Id,
                                        FullName = joined.Employee.FullName,
                                    });
            return await query.ToListAsync(cancellationToken);
        }
    }
}
