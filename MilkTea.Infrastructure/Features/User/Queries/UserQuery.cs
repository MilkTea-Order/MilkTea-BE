using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.User.Abstractions.Queries;
using MilkTea.Application.Features.User.Model.Dtos;
using MilkTea.Infrastructure.Persistence;
using Shared.Extensions;
namespace MilkTea.Infrastructure.Features.User.Queries
{
    public class UserQuery(AppDbContext context) : IUserQuery
    {
        private readonly AppDbContext _vContext = context;

        public async Task<List<UserProfile>> GetUserListAsync(List<int> employeeIds, CancellationToken cancellationToken)
        {
            var query = _vContext.Employees.AsNoTracking()
                                    .Where(x => employeeIds.Contains(x.Id))
                                    .Select(x => new UserProfile
                                    {
                                        EmployeeId = x.Id,
                                        FullName = x.FullName,
                                    });
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<UserProfile?> GetUserProfileByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            var data = await _vContext.Employees
                .AsNoTracking()
                .Where(e => e.Id == employeeId)
                .Select(e => new
                {
                    e.Id,
                    e.Code,
                    e.FullName,
                    e.Avatar,

                    e.GenderID,
                    GenderName = e.Gender != null ? e.Gender.Name : null,

                    e.BirthDay,
                    e.IdentityCode,
                    Email = e.Email != null ? e.Email.Value : null,
                    e.Address,
                    CellPhone = e.CellPhone != null ? e.CellPhone.Value : null,

                    e.PositionID,
                    PositionName = e.Position != null ? e.Position.Name : null,

                    e.Status,
                    e.StartWorkingDate,
                    e.EndWorkingDate,

                    BankName = e.BankAccount != null ? e.BankAccount.BankName : null,
                    BankAccountName = e.BankAccount != null ? e.BankAccount.AccountName : null,
                    BankAccountNumber = e.BankAccount != null ? e.BankAccount.AccountNumber : null,
                    BankQrCode = e.BankAccount != null ? e.BankAccount.QrCode : null,

                    e.CreatedDate,
                    e.UpdatedDate
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (data == null)
                return null;

            return new UserProfile
            {
                EmployeeId = data.Id,
                EmployeeCode = data.Code,
                FullName = data.FullName,
                Avatar = data.Avatar != null
                    ? $"data:image/png;base64,{Convert.ToBase64String(data.Avatar)}"
                    : null,

                GenderId = data.GenderID,
                GenderName = data.GenderName,

                BirthDay = data.BirthDay.ToString(),
                IdentityCode = data.IdentityCode,
                Email = data.Email,
                Address = data.Address,
                CellPhone = data.CellPhone,

                PositionId = data.PositionID,
                PositionName = data.PositionName,

                StatusId = (int)data.Status,
                StatusName = data.Status.GetDescription(),

                StartWorkingDate = data.StartWorkingDate,
                EndWorkingDate = data.EndWorkingDate,

                BankName = data.BankName,
                BankAccountName = data.BankAccountName,
                BankAccountNumber = data.BankAccountNumber,
                BankQrCodeBase64 = data.BankQrCode != null
                    ? $"data:image/png;base64,{Convert.ToBase64String(data.BankQrCode)}"
                    : null,

                CreatedDate = data.CreatedDate,
                LastUpdatedDate = data.UpdatedDate
            };
        }

        public async Task<int?> GetEmployeeIdByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _vContext.Employees
                .AsNoTracking()
                .Where(e => e.Email.Value == email)
                .Select(e => (int?)e.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}