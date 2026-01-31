using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MilkTea.Application.Ports.Catalog;
using MilkTea.Application.Features.Catalog.Queries;
using Shared.Abstractions.Behaviors;
using System.Reflection;

namespace MilkTea.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Register catalog sales query service
        services.AddScoped<ICatalogSalesQuery, CatalogSalesQueryHandler>();

        return services;
    }
}
