using MilkTea.Application.Features.Auth.Abstractions.Services;
using MilkTea.Application.Features.User.Abstractions.Queries;
using MilkTea.Application.Features.User.Model.Dtos;
using MilkTea.Application.Features.User.Model.Results;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.User.Queries
{
    public class GetUserListQuery : IQuery<GetUserListResult> { }
    public class GetUserListQueryHandler(IAuthService authServices, IUserQuery userQuery) : IQueryHandler<GetUserListQuery, GetUserListResult>
    {
        private readonly IAuthService _vAuthService = authServices;
        private readonly IUserQuery _vUserQuery = userQuery;

        public async Task<GetUserListResult> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var result = new GetUserListResult();
            var accounts = await _vAuthService.GetAccountsAsync(cancellationToken);
            var users = await _vUserQuery.GetUserListAsync(accounts.Select(x => x.EmployeeID).ToList(), cancellationToken);

            var accountProfiles = accounts.Select(acc =>
            {
                var user = users.FirstOrDefault(u => u.EmployeeId == acc.EmployeeID);

                return new AccountProfile
                {
                    UserId = acc.UserID,
                    FullName = user?.FullName,
                };
            }).ToList();

            result.Users = accountProfiles;
            return result;
        }
    }
}

//UserName = acc.Username,
//EmployeeId = user?.EmployeeId,
//Avatar = user?.Avatar,
//EmployeeCode = user?.EmployeeCode,
//GenderId = user?.GenderId,
//GenderName = user?.GenderName,
//BirthDay = user?.BirthDay,
//IdentityCode = user?.IdentityCode,
//Email = user?.Email,
//Address = user?.Address,
//CellPhone = user?.CellPhone,
//PositionId = user?.PositionId ?? 0,
//PositionName = user?.PositionName,
//StatusId = user?.StatusId ?? 0,
//StatusName = user?.StatusName,
//StartWorkingDate = user?.StartWorkingDate,
//EndWorkingDate = user?.EndWorkingDate,
//BankName = user?.BankName,
//BankAccountName = user?.BankAccountName,
//BankAccountNumber = user?.BankAccountNumber,
//BankQrCodeBase64 = user?.BankQrCodeBase64,
//CreatedDate = user?.CreatedDate,
//LastUpdatedDate = user?.LastUpdatedDate