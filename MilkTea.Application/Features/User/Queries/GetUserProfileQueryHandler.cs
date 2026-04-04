using MilkTea.Application.Features.Auth.Abstractions.Services;
using MilkTea.Application.Features.User.Abstractions.Queries;
using MilkTea.Application.Features.User.Model.Dtos;
using MilkTea.Application.Features.User.Model.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Common.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.User.Queries;

public sealed class GetUserProfileQuery : IQuery<GetUserProfileResult>
{
}
public sealed class GetUserProfileQueryHandler(IUserQuery userQuery,
                                                 IAuthService authService,
                                                 IIdentifyServicePorts currentUser) : IQueryHandler<GetUserProfileQuery, GetUserProfileResult>
{
    private readonly IUserQuery _vUserQuery = userQuery;
    private readonly IAuthService _vAuthService = authService;
    private readonly IIdentifyServicePorts _vCurrentUser = currentUser;
    public async Task<GetUserProfileResult> Handle(GetUserProfileQuery query, CancellationToken cancellationToken)
    {
        var result = new GetUserProfileResult();
        var userId = _vCurrentUser.UserId;
        var user = await _vAuthService.GetAccountByUserIdAsync(userId, cancellationToken);
        if (user is null)
        {
            result.ResultData.Add(ErrorCode.E0001, "UserId");
            return result;
        }
        var employee = await _vUserQuery.GetUserProfileByEmployeeIdAsync(user.EmployeeID, cancellationToken);
        if (employee is null)
        {
            result.ResultData.Add(ErrorCode.E0001, "UserId");
            return result;
        }

        result.User = new AccountProfile
        {
            UserId = user.UserID,
            UserName = user.Username,

            EmployeeId = employee.EmployeeId,
            Avatar = employee.Avatar,
            EmployeeCode = employee.EmployeeCode,
            FullName = employee.FullName,

            GenderId = employee.GenderId,
            GenderName = employee.GenderName,

            BirthDay = employee.BirthDay,
            IdentityCode = employee.IdentityCode,
            Email = employee.Email,
            Address = employee.Address,
            CellPhone = employee.CellPhone,

            PositionId = employee.PositionId,
            PositionName = employee.PositionName,

            StatusId = employee.StatusId,
            StatusName = employee.StatusName,

            StartWorkingDate = employee.StartWorkingDate,
            EndWorkingDate = employee.EndWorkingDate,

            BankName = employee.BankName,
            BankAccountName = employee.BankAccountName,
            BankAccountNumber = employee.BankAccountNumber,
            BankQrCodeBase64 = employee.BankQrCodeBase64,

            CreatedDate = employee.CreatedDate,
            LastUpdatedDate = employee.LastUpdatedDate
        };
        return result;
    }
}
