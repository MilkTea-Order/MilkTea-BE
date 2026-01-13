using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Database
{
    public static class DatabaseRegistration
    {
        public static IServiceCollection AddConnectDatabase(this IServiceCollection services,
                                                        IConfiguration configuration, IDBProvider provider)
        {
            services.AddDbContext<AppDbContext>(options => provider.ConfigureDbContext(options, "Infrastructure"));
            return services;
        }
    }
}
