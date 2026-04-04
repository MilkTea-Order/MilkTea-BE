using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Configuration.Abstractions.Services;
using MilkTea.Infrastructure.Persistence;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Infrastructure.Features.Configuration.Services
{
    public class ConfigurationService(AppDbContext context) : IConfigurationService
    {
        private readonly AppDbContext _vContext = context;

        public async Task<string?> GetBillPrefix()
        {
            return await _vContext.Definitions.Where(d => d.Code == Denifinitions.BILL_PREFIX_CODE).Select(d => d.Value).FirstOrDefaultAsync();
        }

        public async Task<bool> IsWarehouseManagementMode(CancellationToken cancellationToken)
        {
            return await _vContext.Definitions.Where(d => d.Code == Denifinitions.WAREHOUSE_MANAGEMENT_MODE_CODE)
                                                .Select(d => d.Value)
                                                .FirstOrDefaultAsync(cancellationToken) == "ON";
        }

        public async Task<int> GetOtpExpirationMinutesAsync(CancellationToken cancellationToken = default)
        {
            var value = await _vContext.Definitions
                .Where(d => d.Code == Denifinitions.TIME_EXPIRED_OTP_CODE)
                .Select(d => d.Value)
                .FirstOrDefaultAsync(cancellationToken);

            if (int.TryParse(value, out var minutes))
                return minutes;

            return 5; // Default 5 minutes
        }
    }
}
