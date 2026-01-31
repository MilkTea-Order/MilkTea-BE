using MediatR;
using MilkTea.Application.Features.Users.Queries;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Application.Models.Users;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.Users.Repositories;

namespace MilkTea.Application.Features.Users.Handles;

public sealed class GetUserProfileQueryHandler(
    IUserUnitOfWork userUnitOfWork,
    ICurrentUser currentUser) : IRequestHandler<GetUserProfileQuery, GetUserProfileResult>
{
    public async Task<GetUserProfileResult> Handle(GetUserProfileQuery query, CancellationToken cancellationToken)
    {
        var result = new GetUserProfileResult();
        var userId = currentUser.UserId;

        var user = await userUnitOfWork.Users.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            result.ResultData.Add(ErrorCode.E0001, nameof(userId));
            return result;
        }
        var employee = await userUnitOfWork.Employees.GetByIdWithGenderAndPositionAsync(user.EmployeeID, cancellationToken);
        if (employee is null)
        {
            result.ResultData.Add(ErrorCode.E0001, "Employee");
            return result;
        }

        result.User = new UserProfile
        {
            UserId = user.Id,
            UserName = user.UserName.value,
            EmployeeId = employee.Id,
            EmployeeCode = employee.Code,
            FullName = employee.FullName,
            GenderId = employee.GenderID,
            GenderName = employee.Gender?.Name,
            BirthDay = employee.BirthDay?.Value,
            IdentityCode = employee.IdentityCode,
            Email = employee.Email?.Value,
            Address = employee.Address,
            CellPhone = employee.CellPhone?.Value,
            PositionId = employee.PositionID,
            PositionName = employee.Position?.Name,
            StatusId = (int)employee.Status,
            StatusName = employee.Status.ToString(),
            StartWorkingDate = employee.StartWorkingDate,
            EndWorkingDate = employee.EndWorkingDate,
            BankName = employee.BankAccount?.BankName,
            BankAccountName = employee.BankAccount?.AccountName,
            BankAccountNumber = employee.BankAccount?.AccountNumber,
            BankQrCodeBase64 = employee.BankAccount?.QrCode == null
                ? null
                : $"data:image/png;base64,{Convert.ToBase64String(employee.BankAccount.QrCode)}",
            CreatedDate = employee.CreatedDate,
            LastUpdatedDate = employee.UpdatedDate
        };

        return result;
    }
}
