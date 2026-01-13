using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Users
{
    public class GenderRepository(AppDbContext context) : IGenderRepository
    {
        private readonly AppDbContext _vContext = context;
        public async Task<bool> ExistsGenderAsync(int genderId)
        {
            return await _vContext.Gender
                .AsNoTracking()
                .AnyAsync(x => x.ID == genderId);
        }
    }
}
