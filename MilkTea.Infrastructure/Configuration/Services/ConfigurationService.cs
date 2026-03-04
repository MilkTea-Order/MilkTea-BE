using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Configuration.Abstractions.Services;
using MilkTea.Infrastructure.Persistence;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Infrastructure.Configuration.Services
{
    public class ConfigurationService(AppDbContext context) : IConfigurationService
    {
        private readonly AppDbContext _vContext = context;

        public async Task<string?> GetBillPrefix()
        {
            return await _vContext.Definitions.Where(d => d.Code == Denifitions.BILL_PREFIX_CODE).Select(d => d.Value).FirstOrDefaultAsync();
        }
    }
}
