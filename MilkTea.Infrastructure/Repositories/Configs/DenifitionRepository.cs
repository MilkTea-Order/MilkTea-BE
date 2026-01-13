using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Entities.Config;
using MilkTea.Domain.Respositories.Configs;
using MilkTea.Infrastructure.Persistence;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Infrastructure.Repositories.Configs
{
    public class DenifitionRepository(AppDbContext context) : IDenifitionRepository
    {
        private readonly AppDbContext _vContext = context;
        public async Task<Definition?> GetCodePrefixBill()
        {
            return await _vContext.Definition.FirstOrDefaultAsync(d => d.Code == Denifitions.BILL_PREFIX_CODE);
        }
    }
}
