using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Infrastructure.Persistence;
using MilkTea.Shared.Extensions;


namespace MilkTea.Infrastructure.Repositories.Users
{
    public class PermissionRepository(AppDbContext context) : IPermissionRepository
    {
        private readonly AppDbContext _vContext = context;
        public async Task<List<Dictionary<string, object?>>> GetPermissionsByUserId(int userId)
        {
            // Get permissions of user by user's role
            var row = await (
                from p in _vContext.Permission
                join pd in _vContext.PermissionDetail
                        on p.ID equals pd.PermissionID
                join rd in _vContext.RoleDetail
                        on pd.ID equals rd.PermissionDetailID
                join usr in _vContext.UserAndRole
                        on rd.RoleID equals usr.RoleID
                where usr.UserID == userId
                select new
                {
                    p.Name,
                    pd
                }).ToListAsync();

            var vRows1Dict = row.ToDictList();
            // Get permissions of user by user's direct assigned permissions
            var row2 = await (
                from p in _vContext.Permission
                join pd in _vContext.PermissionDetail
                        on p.ID equals pd.PermissionID
                join upd in _vContext.UserAndPermissionDetail
                        on pd.ID equals upd.PermissionDetailID
                where upd.UserID == userId
                select new
                {
                    p.Name,
                    pd
                }).ToListAsync();
            // Merge two list and remove duplicates
            vRows1Dict.AddRange(row2.ToDictList());
            return vRows1Dict.Distinct().ToList();
        }

    }
}

