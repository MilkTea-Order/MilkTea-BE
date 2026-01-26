using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MilkTea.Infrastructure.Persistence;
using MilkTea.Infrastructure.Persistence.Interceptors;

namespace MilkTea.Infrastructure.Database
{
    public static class DatabaseRegistration
    {
        public static IServiceCollection AddConnectDatabase(this IServiceCollection services,
                                                        IConfiguration configuration, IDBProvider provider)
        {
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var interceptors = sp.GetServices<ISaveChangesInterceptor>();
                options.AddInterceptors(interceptors);
                provider.ConfigureDbContext(options, "Infrastructure");
            });

            return services;
        }
    }
}
