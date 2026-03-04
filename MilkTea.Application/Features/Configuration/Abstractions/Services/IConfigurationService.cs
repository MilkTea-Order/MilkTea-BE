namespace MilkTea.Application.Features.Configuration.Abstractions.Services
{
    public interface IConfigurationService
    {
        Task<string?> GetBillPrefix();
    }
}
