using MediatR;
using MilkTea.Application.DTOs.Users;
using MilkTea.Application.Ports.Users;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.Users.Repositories;

namespace MilkTea.Application.Features.Users.Queries;

public sealed class GetUserProfileQueryHandler(
    IUserRepository userRepository,
    ICurrentUser currentUser) : IRequestHandler<GetUserProfileQuery, GetUserProfileResult>
{
    public async Task<GetUserProfileResult> Handle(GetUserProfileQuery query, CancellationToken cancellationToken)
    {
        var result = new GetUserProfileResult();
        var userId = currentUser.UserId;

        var user = await userRepository.GetWithEmployeeAsync(userId);
        if (user is null || user.Employee is null)
        {
            result.ResultData.Add(ErrorCode.E0001, nameof(userId));
            return result;
        }

        result.User = new UserProfileDto
        {
            UserId = user.Id,
            UserName = user.UserName,
            EmployeeId = user.Employee.Id,
            EmployeeCode = user.Employee.Code,
            FullName = user.Employee.FullName,
            GenderId = user.Employee.GenderID,
            GenderName = user.Employee.Gender?.Name,
            BirthDay = user.Employee.BirthDay,
            IdentityCode = user.Employee.IdentityCode,
            Email = user.Employee.Email,
            Address = user.Employee.Address,
            CellPhone = user.Employee.CellPhone,
            PositionId = user.Employee.PositionID,
            PositionName = user.Employee.Position?.Name,
            StatusId = (int)user.Employee.Status,
            StatusName = user.Employee.Status.ToString(),
            StartWorkingDate = user.Employee.StartWorkingDate,
            EndWorkingDate = user.Employee.EndWorkingDate,
            BankName = user.Employee.BankName,
            BankAccountName = user.Employee.BankAccountName,
            BankAccountNumber = user.Employee.BankAccountNumber,
            BankQrCodeBase64 = user.Employee.BankQRCode == null
                ? null
                : $"data:image/png;base64,{Convert.ToBase64String(user.Employee.BankQRCode)}",
            CreatedDate = user.Employee.CreatedDate,
            LastUpdatedDate = user.Employee.LastUpdatedDate
        };

        return result;
    }
}
