namespace MilkTea.Application.Features.Configuration.Abstractions.Services
{
    public interface IConfigurationService
    {
        Task<string?> GetBillPrefix();
        Task<bool> IsWarehouseManagementMode(CancellationToken cancellationToken = default);
    }
}
